using System.Collections.Generic;

namespace SriSai.API.DTOs.Users
{
    public class UserListDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}