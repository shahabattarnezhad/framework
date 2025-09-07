using Entities.Models.Authentication;

namespace Contracts.Interfaces.Authentication;

public interface IRolePermissionRepository
{
    Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync
        (string roleId, bool trackChanges, CancellationToken cancellationToken = default);

    Task AssignPermissionToRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default);

    Task RemovePermissionFromRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default);

    Task<RolePermission> GetPermissionsAsync
        (string roleId, Guid permissionId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<IEnumerable<Permission>> GetPermissionsByRoleIdsAsync(
        IEnumerable<string> roleIds, bool trackChanges, CancellationToken cancellationToken = default);

    Task<IEnumerable<Permission>> GetPermissionsByRoleNamesAsync(
    IEnumerable<string> roleNames, bool trackChanges, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default);
}
