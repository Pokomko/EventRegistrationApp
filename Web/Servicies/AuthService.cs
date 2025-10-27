using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.Servicies;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly ICookieService _cookieService;
    public AuthService(IUserService userService, ICookieService cookieService)
    {
        _userService = userService;
        _cookieService = cookieService;
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto dto)
    {
        var token = await _userService.LoginAsync(dto.Email, dto.Password);

        _cookieService.SetAuthCookie("kukuha", token);

        return new AuthResultDto("/User/Events");
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        await _userService.RegisterAsync(dto.Firstname, dto.Password, dto.Email);
    }
}
