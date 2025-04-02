using ErrorOr;
using MediatR;
using SriSai.Application.Interfaces.Encryption;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Users.Command;
using SriSai.Domain.Entity.Users;
using SriSai.Domain.Errors;
using SriSai.Domain.Interface;
using System.Security.Cryptography;
using System.Text;
using SriSai.Application.Messaging.Command;
using SriSai.Application.Configuration;

namespace SriSai.Application.Users.Handler
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashPassword _passwordHasher;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMediator _mediator;

        public ResetPasswordCommandHandler(
            IUnitOfWork unitOfWork,
            IHashPassword passwordHasher,
            IDateTimeProvider dateTimeProvider, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _dateTimeProvider = dateTimeProvider;
            _mediator = mediator;
        }

        public async Task<ErrorOr<string>> Handle(
            ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            // Find user by mobile number
            UserEntity? user = await _unitOfWork.Repository<UserEntity>().FindOneAsync(u => u.Mobile == request.Mobile);
            if (user is null)
            {
                return Error.NotFound(PreDefinedErrorsForUsers.UserNotFound);
            }

            // Generate a new random password
            string newPassword = GenerateRandomPassword();
            
            // Hash the new password
            string hashedPassword = _passwordHasher.HashPassword(newPassword);
            
            // Update the user's password
            user.UpdatePassword(hashedPassword);
            user.UpdatedDateTime = _dateTimeProvider.GetUtcNow();
            
            // Save changes
            await _unitOfWork.SaveChangesAsync();
            
             SendMessageCommand createUserMessage = new()
                {
                    MobileNumber = request.Mobile,
                    MessageBody =
                    [
                        user.FirstName + ' ' + user.LastName, request.Mobile,
                        newPassword
                    ],
                    TemplateName = WhatsAppTemplateNames.UserCreatedTemplate
                };
                ErrorOr<Unit> sendMessageResult = await _mediator.Send(createUserMessage, cancellationToken);
            // Return the new password to be sent to the user
            return newPassword;
        }

        private string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(validChars[(int)(num % (uint)validChars.Length)]);
                }
            }

            return res.ToString();
        }
    }
}