using ErrorOr;
using MediatR;
using SriSai.Application.Interfaces.Encryption;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;
using SriSai.Domain.Errors;

namespace SriSai.Application.Users.Handler;

public class ValidateUserQueryHandler
    : IRequestHandler<ValidateUserQuery, ErrorOr<UserProfileResponse>>
{
    private readonly IVerifyPassword _passwordVerifier;
    private readonly IRepository<UserEntity> _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;

    public ValidateUserQueryHandler(IRepository<UserEntity> userRepository, IVerifyPassword passwordVerifier,
        IRepository<UserRole> userRoleRepository)
    {
        _userRepository = userRepository;
        _passwordVerifier = passwordVerifier;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<ErrorOr<UserProfileResponse>> Handle(
        ValidateUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = (await _userRepository.ListAsync(u => u.Mobile == request.Mobile)).FirstOrDefault();
        if (user is null) return Error.Forbidden(PreDefinedErrorsForUsers.UserNotFound);

        if (!_passwordVerifier.VerifyPassword(request.Password, user.Password))
            return Error.Forbidden(PreDefinedErrorsForUsers.UserNotFound);
        var roles = await _userRoleRepository.ListAsync(z => z.UserEntityId == user.Id);
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