using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces;
using Application.Services;
using Application.DTO;
using Web.Servicies;

namespace Web.Endpoints;

public static class UserEndPoints
{
    public static IEndpointRouteBuilder MapUsersEndPoints(this IEndpointRouteBuilder app) {
        app.MapPost("api/register", Register);
        app.MapPost("api/login", Login);
        app.MapPost("api/logout", Logout);

        return app;
    }

    public static async Task<IResult> Register (RegisterDto dto, IAuthService authService) 
    {
        await authService.RegisterAsync(dto);

        var loginDto = new LoginDto(dto.Email, dto.Password);
        var result = await authService.LoginAsync(loginDto);

        return Results.Ok(new { redirect = result.RedirectUrl });
    }

    public static async Task<IResult> Login(LoginDto dto, IAuthService authService)
    {
        var result = await authService.LoginAsync(dto);

        return Results.Ok(new { redirect = result.RedirectUrl });
    }

    public static IResult Logout(ICookieService cookieService)
    {
        cookieService.DeleteAuthCookie("kukuha");

        return Results.Ok(new { redirect = "api/Login" });
    }
}
