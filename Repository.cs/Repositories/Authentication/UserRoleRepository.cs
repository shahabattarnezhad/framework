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
        var exists = await FindByCondition(ur => ur.UserId == userId && ur.RoleId == roleId, false)
            .AnyAsync(cancellationToken);

        if (!exists)
        {
            Create(new UserRole
            {
                UserId = userId,
                RoleId = roleId
            });
        }
    }

    public async Task RemoveRoleFromUserAsync(string userId, string roleId, CancellationToken cancellationToken = default)
    {
        var entity = await FindByCondition(ur => ur.UserId == userId && ur.RoleId == roleId, true)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity != null)
            Delete(entity);
    }

    public async Task<List<Role>> GetRolesByUserIdAsync
        (string userId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(ur => ur.UserId == userId, trackChanges)
            .Select(ur => ur.Role!)
            .ToListAsync(cancellationToken);
    }
}
