using ErrorOr;
using MediatR;
using SriSai.Domain.Entity.Users;

namespace SriSai.Application.Users.Command
{
    public class CreateUserCommand : IRequest<ErrorOr<Guid>>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Mobile { get; set; }
        public required IList<UserRoleEntity> Roles { get; set; }
        public required Guid CreatedById { get; set; }
    }
}