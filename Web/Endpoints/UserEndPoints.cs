using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Services;
using Web.DTO;

namespace Web.Endpoints;

public static class UserEndPoints
{
    public static IEndpointRouteBuilder MapUsersEndPoints(this IEndpointRouteBuilder app) {
        app.MapPost("api/register", Register);
        app.MapPost("api/login", Login);
        app.MapPost("api/logout", Logout);

        return app;
    }

    public static async Task<IResult> Register (RegisterDto dto, UserService userService, HttpContext context) {
        await userService.Register(dto.Username, dto.Password, dto.Email);

        var token = await userService.Login(dto.Email, dto.Password);
        context.Response.Cookies.Append("kukuha", token);

        return Results.Ok();
    }

    public static async Task<IResult> Login(LoginDto dto, UserService userService, HttpContext context)
    {
        var token = await userService.Login(dto.Email,dto.Password);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var roles = jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var role = roles.FirstOrDefault(); // или логика выбора основной роли

        string redirectUrl = role switch
        {
            "Admin" => "/admin/dashboard",
            "User" => "/user/home",
            _ => "/"
        };

        context.Response.Cookies.Append("kukuha", token);

        return Results.Ok(new { redirect = redirectUrl });
    }

    public static IResult Logout(HttpContext context)
    {
        context.Response.Cookies.Append("kukuha", "", new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(-1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return Results.Ok(new { redirect = "api/Login" });
    }
}
