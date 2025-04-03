using ErrorOr;
using MediatR;
using SriSai.Application.Configuration;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Interfaces.Encryption;
using SriSai.Application.Messaging.Command;
using SriSai.Application.Users.Command;
using SriSai.Domain.Entity.Users;
using SriSai.Domain.Errors;
using SriSai.Domain.Interface;

namespace SriSai.Application.Users.Handler
{
    public class CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IHashPassword hashPassword,
        IMediator mediator)
        : IRequestHandler<CreateUserCommand, ErrorOr<Guid>>
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IHashPassword _hashPassword = hashPassword;
        private readonly IMediator _mediator = mediator;

        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Guid>> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                UserEntity? userExist =
                    await _unitOfWork.Repository<UserEntity>().FindOneAsync(x => x.Mobile == request.Mobile);
                if (userExist != null)
                {
                    return Error.Conflict(PreDefinedErrorsForUsers.UserAlreadyExist);
                }

                UserEntity user = new(_dateTimeProvider);
                string hashedPassword = _hashPassword.HashPassword(request.Password);
                user.AddNewUser(request.FirstName, request.LastName, request.Email, hashedPassword, request.Mobile,
                    request.Roles, request.CreatedById);
                UserEntity result = await _unitOfWork.Repository<UserEntity>().AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                SendMessageCommand createUserMessage = new()
                {
                    MobileNumber = request.Mobile,
                    MessageBody =
                    [
                        request.FirstName + ' ' + request.LastName, request.Mobile,
                        request.Password
                    ],
                    TemplateName = WhatsAppTemplateNames.UserCreatedTemplate
                };
                ErrorOr<Unit> sendMessageResult = await _mediator.Send(createUserMessage, cancellationToken);
                return user.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}