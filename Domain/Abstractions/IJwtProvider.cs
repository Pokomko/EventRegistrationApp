using Domain.Entities;

namespace Domain.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}