using MediatR;
using SriSai.Application.Users.Dtos; // Use the Application layer DTO
using System.Collections.Generic;

namespace SriSai.Application.Users.Query
{
    // This query requests a list of UserListResponse objects
    public class GetAllUsersQuery : IRequest<List<UserListResponse>>
    {
    }
}