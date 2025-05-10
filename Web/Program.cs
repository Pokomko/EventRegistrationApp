using Application.Services;
using Domain.Abstractions;
using Domain.Enum;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Web.Endpoints;
using Web.Extensions;
using Web.Helpers;

namespace Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
        builder.Services.Configure<AuthorizationOptions>(builder.Configuration.GetSection(nameof(AuthorizationOptions)));

/*        var config = builder.Configuration.GetSection("AuthorizationOptions").Get<AuthorizationOptions>();
        var parsed = RolePermissionParser.Parse(config);

        foreach (var rp in parsed)
        {
            Console.WriteLine($"Parsed: RoleId={rp.RoleId}, PermissionId={rp.PermissionId}");
        }*/

        builder.Services
            .AddApiAuthintication(builder.Configuration);

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
                policy.Requirements.Add(new PermissionRequirment([PermissionsEnum.Create])));
        });

        // Add services to the container.
        builder.AddInfrastructureServices();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<IJwtProvider, JwtProvider>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapUsersEndPoints();

        app.UseStaticFiles();
        app.UseDefaultFiles();

        app.MapControllers();

        app.Run();
    }
}
