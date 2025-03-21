using ErrorOr;
using MediatR;
using SriSai.Application.Interfaces.Encryption;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;
using SriSai.Domain.Errors;

namespace SriSai.Application.Users.Handler
{
    public class ValidateUserQueryHandler
        : IRequestHandler<ValidateUserQuery, ErrorOr<UserProfileResponse>>
    {
        private readonly IVerifyPassword _passwordVerifier;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IRepository<UserRoleEntity> _userRoleRepository;

        public ValidateUserQueryHandler(IRepository<UserEntity> userRepository, IVerifyPassword passwordVerifier,
            IRepository<UserRoleEntity> userRoleRepository)
        {
            _userRepository = userRepository;
            _passwordVerifier = passwordVerifier;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<ErrorOr<UserProfileResponse>> Handle(
            ValidateUserQuery request,
            CancellationToken cancellationToken)
        {
            UserEntity? user = await _userRepository.FindOneAsync(u => u.Mobile == request.Mobile);
            if (user is null)
            {
                return Error.Forbidden(PreDefinedErrorsForUsers.UserNotFound);
            }

            if (!_passwordVerifier.VerifyPassword(request.Password, user.Password))
            {
                return Error.Forbidden(PreDefinedErrorsForUsers.UserNotFound);
            }

            IEnumerable<UserRoleEntity> roles =
                await _userRoleRepository.FindAllForConditionAsync(z => z.UserEntityId == user.Id);
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