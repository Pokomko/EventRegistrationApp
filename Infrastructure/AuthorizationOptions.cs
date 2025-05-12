namespace Infrastructure;

public class AuthorizationOptions
{
    public RolePermissionsSet[] RolePermissions { get; set; } = [];
}

public class RolePermissionsSet
{ 
    public string Role { get; set; } = string.Empty;

    public string[] Permissions { get; set; } = [];
}
