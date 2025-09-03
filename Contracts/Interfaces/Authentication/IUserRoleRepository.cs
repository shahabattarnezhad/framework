using Entities.Models.Authentication;

namespace Contracts.Interfaces.Authentication;

public interface IUserRoleRepository
{
    Task AssignRoleToUserAsync(string userId, string roleId, CancellationToken cancellationToken = default);

    Task RemoveRoleFromUserAsync(string userId, string roleId, CancellationToken cancellationToken = default);

    Task<List<Role>> GetRolesByUserIdAsync(string userId, bool trackChanges, CancellationToken cancellationToken = default);
}
