using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Net;

namespace Infrastructure.Configurations;

internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    private readonly AuthorizationOptions _authorization;

    public RolePermissionConfiguration(AuthorizationOptions authorization)
    {
        _authorization = authorization;
    }
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(r => new { r.RoleId, r.PermissionId });

        builder.HasData(ParseRolePermissions());
    }

    private RolePermission[] ParseRolePermissions()
    {
        return _authorization.RolePermissions
            .SelectMany(rp => rp.Permissions
                .Select(p => new RolePermission
                {
                    RoleId = (int)Enum.Parse<RolesEnum>(rp.Role),
                    PermissionId = (int)Enum.Parse<PermissionsEnum>(p)
                }))
                .ToArray();
    }
}
