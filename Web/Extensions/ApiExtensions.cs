using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        services.AddAuthorization(options => {
            options.AddPolicy("AdminPolicy", policy => {
                policy.RequireClaim("Admin", "true");
            });
        });
    }
}
