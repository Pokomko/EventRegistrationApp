using System.Text;
using Application.Services;
using Domain.Abstractions;
using Domain.Enum;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Web.Extensions;

public static class ApiExtensions
{

    public static void AddApiAuthintication(
        this IServiceCollection services,
        IConfiguration configuration) {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var jwtOptions = configuration
                .GetSection("JwtOptions")
                .Get<JwtOptions>() ??
                throw new InvalidDataException("JwtOPtions is empty");

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                };

                options.Events = new JwtBearerEvents {
                    OnMessageReceived = context => {
                        var token = context.Request.Cookies["kukuha"];
                        context.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthorization();
    }

    public static IEndpointConventionBuilder RequirePermissions<TBuilder>(
        this TBuilder builder, params PermissionsEnum[] permissions)
            where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireAuthorization(policy =>
            policy.AddRequirements(new PermissionRequirment(permissions)));
    }
}
