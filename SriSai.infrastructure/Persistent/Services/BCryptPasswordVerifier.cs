using SriSai.Application.Interfaces.Encryption;

namespace SriSai.infrastructure.Persistent.Services;

public class BCryptPasswordVerifier : IVerifyPassword
{
    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}