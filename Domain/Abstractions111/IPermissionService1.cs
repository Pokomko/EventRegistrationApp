using Domain.Enum;

namespace Domain.Abstractions;

public interface IPermissionService1
{
    Task<HashSet<PermissionsEnum>> GetPermissionsAsync(Guid id);
}
