namespace SriSai.API.DTOs.Users;

public class UserInformationDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Mobile { get; set; }
    public required string JwtToken { get; set; }
}