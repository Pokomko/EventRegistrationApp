using Domain.Abstractions;
using Domain.Entities;
using Domain.Enum;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        var role = await _context.Role
            .SingleOrDefaultAsync(r => r.Id == (int)RolesEnum.User)
            ?? throw new InvalidOperationException();

        user.Roles.Add(role);

        await _context.Users.AddAsync(user);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<HashSet<PermissionsEnum>> GetUserPermissions(Guid userId) {
        var roles = await _context.Users
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => (PermissionsEnum)p.Id)
            .ToHashSet();
    }
}
