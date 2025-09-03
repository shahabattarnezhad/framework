using Contracts.Interfaces.Authentication;
using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;

namespace Repository.Repositories.Authentication;

public class RolePermissionRepository : RepositoryBase<RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(RepositoryContext context) : base(context) { }

    public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(string roleId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await RepositoryContext.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.Permission!)
            .ToListAsync(cancellationToken);
    }

    public async Task AssignPermissionToRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        var exists = await RepositoryContext.RolePermissions
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);

        if (!exists)
        {
            await RepositoryContext.RolePermissions.AddAsync(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            }, cancellationToken);
        }
    }

    public async Task RemovePermissionFromRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        var entity = await RepositoryContext.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);

        if (entity != null)
            RepositoryContext.RolePermissions.Remove(entity);
    }
}
