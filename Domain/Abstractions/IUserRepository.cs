using Domain.Entities;
using Domain.Enum;

namespace Domain.Abstractions;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task SaveChangesAsync();
    Task<HashSet<PermissionsEnum>> GetUserPermissions(Guid userId);
}
