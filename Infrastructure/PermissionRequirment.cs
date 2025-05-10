using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure;

public class PermissionRequirment : IAuthorizationRequirement
{
    public PermissionsEnum[] Permissions { get; set; } = [];

    public PermissionRequirment(PermissionsEnum[] permissions)
    {
        Permissions = permissions;
    }
}
