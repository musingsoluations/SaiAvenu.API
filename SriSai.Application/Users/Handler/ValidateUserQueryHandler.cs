using ErrorOr;
using MediatR;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Interfaces.Encryption;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;
using SriSai.Domain.Errors;

namespace SriSai.Application.Users.Handler
{
    public class ValidateUserQueryHandler
        : IRequestHandler<ValidateUserQuery, ErrorOr<UserProfileResponse>>
    {
        private readonly IVerifyPassword _passwordVerifier;

        private readonly IUnitOfWork _unitOfWork;

        public ValidateUserQueryHandler(IVerifyPassword passwordVerifier,
            IUnitOfWork unitOfWork)
        {
            //_userRepository = userRepository;
            _passwordVerifier = passwordVerifier;
            //_userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<UserProfileResponse>> Handle(
            ValidateUserQuery request,
            CancellationToken cancellationToken)
        {
            UserEntity? user = await _unitOfWork.Repository<UserEntity>().FindOneAsync(u => u.Mobile == request.Mobile);
            if (user is null)
            {
                return Error.Forbidden(PreDefinedErrorsForUsers.UserNotFound);
            }

            if (!_passwordVerifier.VerifyPassword(request.Password, user.Password))
            {
                return Error.Forbidden(PreDefinedErrorsForUsers.UserNotFound);
            }

            IEnumerable<UserRoleEntity> roles =
                await _unitOfWork.Repository<UserRoleEntity>().FindAllForConditionAsync(z => z.UserEntityId == user.Id);
            user.Roles = roles.ToList();
            return new UserProfileResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Mobile,
                user.Roles.Select(r => r.UserRoleName).ToList());
        }
    }
}