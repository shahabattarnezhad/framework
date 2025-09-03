using Contracts.Interfaces.Authentication;
using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;

namespace Repository.Repositories.Authentication;

public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(RepositoryContext context) : base(context) { }

    public async Task AssignRoleToUserAsync(string userId, string roleId, CancellationToken cancellationToken = default)
    {
        var exists = await RepositoryContext.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);

        if (!exists)
        {
            await RepositoryContext.UserRoles.AddAsync(new UserRole
            {
                UserId = userId,
                RoleId = roleId
            }, cancellationToken);
        }
    }

    public async Task RemoveRoleFromUserAsync(string userId, string roleId, CancellationToken cancellationToken = default)
    {
        var entity = await RepositoryContext.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);

        if (entity != null)
            RepositoryContext.UserRoles.Remove(entity);
    }

    public async Task<List<Role>> GetRolesByUserIdAsync
        (string userId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await RepositoryContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role!)
            .ToListAsync(cancellationToken);
    }
}
