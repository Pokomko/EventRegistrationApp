using Domain.Abstractions;
using Domain.Entities;
using Domain.Enum;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRepository> _logger; // Логгер для репозитория

    public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task AddAsync(User user)
    {
        try
        {
            _logger.LogInformation("Начинаем добавление пользователя: {Username}", user.Username);

            // Поиск роли пользователя
            var role = await _context.Role
                .SingleOrDefaultAsync(r => r.Id == (int)RolesEnum.User)
                ?? throw new InvalidOperationException("Роль не найдена");

            user.Roles.Add(role);

            // Добавление пользователя
            await _context.Users.AddAsync(user);
            _logger.LogInformation("Пользователь {Username} успешно добавлен.", user.Username);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении пользователя: {Username}", user.Username);
            throw; // Перебрасываем исключение дальше
        }
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        try
        {
            _logger.LogInformation("Получение пользователя по email: {Email}", email);

            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                _logger.LogInformation("Пользователь с email {Email} найден.", email);
            }
            else
            {
                _logger.LogWarning("Пользователь с email {Email} не найден.", email);
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении пользователя по email: {Email}", email);
            throw;
        }
    }


    public async Task<User?> GetByIdAsync(Guid userId)
    {
        try
        {
            _logger.LogInformation("Получение пользователя по ID: {UserId}", userId);

            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                _logger.LogInformation("Пользователь с ID {UserId} найден.", userId);
            }
            else
            {
                _logger.LogWarning("Пользователь с ID {UserId} не найден.", userId);
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении пользователя по ID: {UserId}", userId);
            throw;
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            _logger.LogInformation("Начинаем сохранение изменений в базе данных.");

            await _context.SaveChangesAsync();

            _logger.LogInformation("Изменения успешно сохранены.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при сохранении изменений в базе данных.");
            throw;
        }
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
