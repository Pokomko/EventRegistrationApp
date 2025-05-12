using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Domain.Abstractions;

namespace Infrastructure;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirment>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirment requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(
                c => c.Type == CustomClaims.UserId);

        if (userId == null || !Guid.TryParse(userId.Value, out var id)) {
            return;
        }
        
        using var scope = _serviceScopeFactory.CreateScope();

        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        var permission = await permissionService.GetPermissionsAsync(id);

        if (permission.Intersect(requirement.Permissions).Any()) {
            context.Succeed(requirement);
        }
    }
}
