using Domain.Entities;

namespace Domain.Abstractions;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByUsernameAsync(string username);
    Task SaveChangesAsync();
}
