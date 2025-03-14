using ErrorOr;
using MediatR;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;
using SriSai.Domain.Errors;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SriSai.Application.Users.Handler;

public class GetUserWithSpecifiedRoleQueryHandler
    : IRequestHandler<GetUserWithSpecifiedRoleQuery, List<UserWithRoleResponse>>
{
    private readonly IRepository<UserEntity> _userRepository;

    public GetUserWithSpecifiedRoleQueryHandler(IRepository<UserEntity> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserWithRoleResponse>> Handle(
        GetUserWithSpecifiedRoleQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Roles == null || !request.Roles.Any())
        {
            return new List<UserWithRoleResponse>();
        }

        var users = await _userRepository.ListAllAsync(x =>
            x.Roles.Any(userRole => request.Roles.Contains(userRole.UserRoleName)));

        return users
            .Select(u => new UserWithRoleResponse(
                u.Id.ToString(),
                u.FirstName,
                u.LastName
            ))
            .ToList();
    }
}