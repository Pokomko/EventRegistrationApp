using Application.Services;
using Domain.Entities;
using Domain.Enum;
using Infrastructure;
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
        await userService.Register(dto.Username, dto.Password, dto.Email);

        var token = await userService.Login(dto.Email, dto.Password);
        context.Response.Cookies.Append("kukuha", token);

        return Results.Ok();
    }

    public static async Task<IResult> Login(LoginDto dto, UserService userService, HttpContext context)
    {
        var token = await userService.Login(dto.Email,dto.Password);

        context.Response.Cookies.Append("kukuha", token);

        return Results.Ok();
    }
}
