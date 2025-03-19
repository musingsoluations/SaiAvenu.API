using SriSai.Domain.Entity.Base;

namespace SriSai.Domain.Entity.Users
{
    public class UserRoleEntity : EntityBase
    {
        public required string UserRoleName { get; set; }
        public Guid UserEntityId { get; set; }
    }
}