using MediatR;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;

namespace SriSai.Application.Users.Handler
{
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
            if (!request.Roles.Any())
            {
                return new List<UserWithRoleResponse>();
            }

            IEnumerable<UserEntity> users = await _userRepository.ListAllForConditionAsync(x =>
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
}