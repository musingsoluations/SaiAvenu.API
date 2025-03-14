using MediatR;
using SriSai.Domain.Entity.Users;
using System.Collections.Generic;

namespace SriSai.Application.Users.Query
{
    public record GetUserWithSpecifiedRoleQuery(
        List<string> Roles
    ) : IRequest<List<UserWithRoleResponse>>;

    public record UserWithRoleResponse(
        string UserId,
        string FirstName,
        string LastName
    );
}