using Shared.DTOs.Authentication;

namespace Service.Contracts.Interfaces.Authentication;

public interface IRolePermissionService
{
    Task<IEnumerable<PermissionDto>> GetPermissionsByRoleIdAsync
        (string roleId, bool trackChanges, CancellationToken cancellationToken = default);

    Task AssignPermissionToRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default);

    Task RemovePermissionFromRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default);
}
