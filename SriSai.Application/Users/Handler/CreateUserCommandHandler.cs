using ErrorOr;
using MediatR;
using SriSai.Application.Interfaces.Encryption;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Users.Command;
using SriSai.Domain.Entity.Users;
using SriSai.Domain.Errors;
using SriSai.Domain.Interface;

namespace SriSai.Application.Users.Handler
{
    public class CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<UserEntity> userRepository,
        IDateTimeProvider dateTimeProvider,
        IHashPassword hashPassword)
        : IRequestHandler<CreateUserCommand, ErrorOr<Guid>>
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IHashPassword _hashPassword = hashPassword;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRepository<UserEntity> _userRepository = userRepository;

        public async Task<ErrorOr<Guid>> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                UserEntity? userExist = await _userRepository.FindOneAsync(x => x.Mobile == request.Mobile);
                if (userExist != null)
                {
                    return Error.Conflict(PreDefinedErrorsForUsers.UserAlreadyExist);
                }

                UserEntity user = new(_dateTimeProvider);
                string hashedPassword = _hashPassword.HashPassword(request.Password);
                user.AddNewUser(request.FirstName, request.LastName, request.Email, hashedPassword, request.Mobile,
                    request.Roles, request.CreatedById);
                UserEntity result = await _userRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
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