using Application.Services;
using Web.DTO;

namespace Web.Endpoints;

public static class UserEndPoints
{
    public static IEndpointRouteBuilder MapUsersEndPoints(this IEndpointRouteBuilder app) {
        app.MapPost("register", Register);
        app.MapPost("login", Login);

        return app;
    }

    public static async Task<IResult> Register (RegisterDto dto, UserService userService, HttpContext context) {
        await userService.Register(dto.Username, dto.Password);

        var token = await userService.Login(dto.Username, dto.Password);
        context.Response.Cookies.Append("kukuha", token);

        return Results.Ok();
    }

    public static async Task<IResult> Login(LoginDto dto, UserService userService, HttpContext context)
    {
        var token = await userService.Login(dto.Username,dto.Password);

        context.Response.Cookies.Append("kukuha", token);

        return Results.Ok();
    }
}
