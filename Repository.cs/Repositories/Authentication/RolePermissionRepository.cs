using Contracts.Interfaces.Authentication;
using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;

namespace Repository.Repositories.Authentication;

public class RolePermissionRepository : RepositoryBase<RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(RepositoryContext context) : base(context) { }

    public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync
        (string roleId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(rp => rp.RoleId == roleId, trackChanges)
                     .Include(rp => rp.Permission)
                     .Select(rp => rp.Permission!)
                     .ToListAsync(cancellationToken);
    }

    public async Task AssignPermissionToRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        var exists = await ExistsAsync(roleId, permissionId, cancellationToken);

        if (!exists)
        {
            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            Create(rolePermission);
        }
    }

    public async Task RemovePermissionFromRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        var entity =
            await FindByCondition(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, true)
                  .FirstOrDefaultAsync(cancellationToken);

        if (entity != null)
            Delete(entity);
    }

    public async Task<bool> ExistsAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, false)
                    .AnyAsync(cancellationToken);
    }
}
