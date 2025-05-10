using Domain.Enum;

namespace Domain.Abstractions;

public interface IPermissionService
{
    Task<HashSet<PermissionsEnum>> GetPermissionsAsync(Guid id);
}
