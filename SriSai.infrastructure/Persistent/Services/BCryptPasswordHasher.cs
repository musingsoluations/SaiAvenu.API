using SriSai.Application.Interfaces.Encryption;

namespace SriSai.infrastructure.Persistent.Services;

public class BCryptPasswordHasher : IHashPassword
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
    }
}