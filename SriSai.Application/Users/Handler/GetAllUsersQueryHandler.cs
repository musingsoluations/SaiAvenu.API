using MediatR;
using SriSai.Application.interfaces.Reposerty;
using SriSai.Application.Users.Dtos;
using SriSai.Application.Users.Query;
using SriSai.Domain.Entity.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SriSai.Application.Users.Handler
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserListResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserListResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // Fetch all non-deleted users, including their roles using the repository method
            var users = await _unitOfWork.Repository<UserEntity>()
                                         .FindAllWithIncludeAsync(
                                             predicate: u => !u.IsDeleted, // Filter for non-deleted users
                                             includes: u => u.Roles        // Include the Roles navigation property
                                         );

            // Map UserEntity to UserListResponse
            // Convert IEnumerable to List if needed downstream, or keep as IEnumerable if sufficient
            var response = users.Select(user => new UserListResponse
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Mobile = user.Mobile,
                Roles = user.Roles.Select(role => role.UserRoleName).ToList() ?? new List<string>() // Handle null Roles collection just in case
            }).ToList();

            return response;
        }
    }
}