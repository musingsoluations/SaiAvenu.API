using ErrorOr;
using MediatR;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;
using SriSai.Domain.Errors;

namespace SriSai.Application.Users.Handler;

public class UserProfileQueryHandler : IRequestHandler<UserProfileQuery, ErrorOr<UserProfile>>
{
    private readonly IRepository<UserEntity> _userRepository;

    public UserProfileQueryHandler(IRepository<UserEntity> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<UserProfile>> Handle(UserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.userId);
        if (user is null) return Error.Forbidden(PreDefinedErrorsForUsers.UserNotFound);

        return new UserProfile(
            user.FirstName,
            user.LastName,
            user.Email,
            user.Mobile);
    }
}