using Domain.Abstractions;
using Domain.Enum;

namespace Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IUserRepository _userRepository;

    public PermissionService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<HashSet<PermissionsEnum>> GetPermissionsAsync(Guid id) {
        return _userRepository.GetUserPermissions(id);
    }
}
