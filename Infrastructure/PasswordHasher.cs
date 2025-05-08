using Domain.Abstractions;

namespace Infrastructure;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
    {
        var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        return hashedPassword;
    }

    public bool Verify(string password, string hashedPassword)
    {
        var verified = BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        return verified;
    }
}
