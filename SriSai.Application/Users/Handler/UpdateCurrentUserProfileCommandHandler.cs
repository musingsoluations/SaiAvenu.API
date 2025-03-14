using ErrorOr;
using MediatR;
using SriSai.Application.Interfaces.Encryption;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Users.Command;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;
using SriSai.Domain.Errors;

namespace SriSai.Application.Users.Handler
{
    public class
        UpdateCurrentUserProfileCommandHandler : IRequestHandler<UpdateCurrentUserProfileCommand, ErrorOr<UserProfile>>
    {
        private readonly IHashPassword _hashPassword;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;

        public UpdateCurrentUserProfileCommandHandler(IRepository<UserEntity> userRepository, IUnitOfWork unitOfWork,
            IHashPassword hashPassword)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _hashPassword = hashPassword;
        }

        public async Task<ErrorOr<UserProfile>> Handle(UpdateCurrentUserProfileCommand request,
            CancellationToken cancellationToken)
        {
            UserEntity? currentData = await _userRepository.GetByIdAsync(request.Id);
            if (currentData is null)
            {
                return Error.Forbidden(PreDefinedErrorsForUsers.UserNotFound);
            }

            if (request.Password is not null && request.Password != string.Empty)
            {
                string hashedPassword = _hashPassword.HashPassword(request.Password);
                if (currentData.Password != hashedPassword)
                {
                    return Error.Validation(PreDefinedErrorsForUsers.InvalidPassword);
                }

                request.Password = hashedPassword;
                currentData.UpdatePassword(request.Password);
            }

            currentData.UpdateFirstName(request.FirstName);
            currentData.UpdateLastName(request.LastName);
            currentData.UpdateEmail(request.Email);
            currentData.UpdateMobile(request.Mobile);
            currentData.UpdateEntityInternals(request.Id);
            await _userRepository.UpdateAsync(currentData);
            await _unitOfWork.SaveChangesAsync();

            return new UserProfile(
                currentData.FirstName,
                currentData.LastName,
                currentData.Email,
                currentData.Mobile);
        }
    }
}