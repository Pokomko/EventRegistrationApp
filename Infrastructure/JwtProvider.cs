using Domain.Entities;
using Domain.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    private readonly ILogger<JwtProvider> _logger;

    // Внедрение ILogger в конструктор
    public JwtProvider(IOptions<JwtOptions> options, ILogger<JwtProvider> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public string GenerateToken(User user)
    {
        _logger.LogInformation("Начало генерации токена для пользователя: {UserId}", user.Id);

        var claims = new List<Claim> {
            new(CustomClaims.UserId, user.Id.ToString())
        };

        _logger.LogDebug("Добавление claims для пользователя: {UserId}", user.Id);

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
            _logger.LogDebug("Добавлен claim для роли: {RoleName}", role.Name);
        }

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256);

        _logger.LogInformation("Используем секретный ключ для подписи токена");

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

        _logger.LogInformation("Токен сгенерирован, срок действия: {ExpiresIn} часов", _options.ExpiresHours);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogInformation("Токен для пользователя {UserId} успешно сгенерирован", user.Id);

        return tokenValue.ToString();
    }
}
