using Domain.Entities;
using Domain.Enum;

namespace Domain.Abstractions;

public interface IUserRepository1
{
    Task AddAsync(User user);
    //Task RegisterAsync(string username, string email, string password);
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByEmailAsync(string email);
    Task SaveChangesAsync();
    Task<HashSet<PermissionsEnum>> GetUserPermissions(Guid userId);
}