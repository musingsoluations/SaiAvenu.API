using ErrorOr;
using MediatR;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Interfaces.Encryption;
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
        //private readonly IRepository<UserEntity> _userRepository;

        public UpdateCurrentUserProfileCommandHandler(IUnitOfWork unitOfWork,
            IHashPassword hashPassword)
        {
            // _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _hashPassword = hashPassword;
        }

        public async Task<ErrorOr<UserProfile>> Handle(UpdateCurrentUserProfileCommand request,
            CancellationToken cancellationToken)
        {
            UserEntity? currentData = await _unitOfWork.Repository<UserEntity>().GetByIdAsync(request.Id);
            if (currentData is null)
            {
                return Error.Forbidden(PreDefinedErrorsForUsers.UserNotFound);
            }

            if (request.Password is not null && request.Password != string.Empty)
            {
                string hashedPassword = _hashPassword.HashPassword(request.Password);
                if (currentData.Password == hashedPassword)
                {
                    return Error.Validation(PreDefinedErrorsForUsers.InvalidPassword);
                }

                request.Password = hashedPassword;
                currentData.UpdatePassword(request.Password);
            }

            UserEntity? findUserByMobile =
                await _unitOfWork.Repository<UserEntity>().FindOneAsync(x => x.Mobile == request.Mobile);
            if (findUserByMobile is not null && findUserByMobile.Id != currentData.Id)
            {
                return Error.Validation(PreDefinedErrorsForUsers.MobileAlreadyExists);
            }

            currentData.UpdateFirstName(request.FirstName);
            currentData.UpdateLastName(request.LastName);
            currentData.UpdateEmail(request.Email);
            currentData.UpdateMobile(request.Mobile);
            currentData.UpdateEntityInternals(request.Id);
            await _unitOfWork.Repository<UserEntity>().UpdateAsync(currentData);
            await _unitOfWork.SaveChangesAsync();

            return new UserProfile(
                currentData.FirstName,
                currentData.LastName,
                currentData.Email,
                currentData.Mobile);
        }
    }
}