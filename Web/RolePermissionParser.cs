using Domain.Entities;
using Domain.Enum;
using Infrastructure;

namespace Web.Helpers;

public static class RolePermissionParser
{
    public static RolePermission[] Parse(AuthorizationOptions options)
    {
        return options.RolePermissions
            .SelectMany(rp => rp.Permissions
                .Select(p => new RolePermission
                {
                    RoleId = (int)Enum.Parse<RolesEnum>(rp.Role),
                    PermissionId = (int)Enum.Parse<PermissionsEnum>(p)
                }))
            .ToArray();
    }
}
