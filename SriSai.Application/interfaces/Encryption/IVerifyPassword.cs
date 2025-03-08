namespace SriSai.Application.Interfaces.Encryption;

public interface IVerifyPassword
{
    bool VerifyPassword(string password, string hashedPassword);
}