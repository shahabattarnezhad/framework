using Entities.Models.Authentication;

namespace Contracts.Interfaces.Authentication;

public interface IRolePermissionRepository
{
    Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync
        (string roleId, bool trackChanges, CancellationToken cancellationToken = default);

    Task AssignPermissionToRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default);

    Task RemovePermissionFromRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default);
}
