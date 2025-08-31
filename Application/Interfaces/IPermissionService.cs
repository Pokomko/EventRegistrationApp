using Domain.Enum;

namespace Application.Interfaces;

public interface IPermissionService
{
    Task<HashSet<PermissionsEnum>> GetPermissionsAsync(Guid id);
}
