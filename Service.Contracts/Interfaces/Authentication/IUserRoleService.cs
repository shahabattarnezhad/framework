using Shared.DTOs.Authentication;

namespace Service.Contracts.Interfaces.Authentication;

public interface IUserRoleService
{
    Task AssignRoleToUserAsync(string userId, string roleId, CancellationToken cancellationToken = default);

    Task RemoveRoleFromUserAsync(string userId, string roleId, CancellationToken cancellationToken = default);

    Task<List<RoleDto>> GetRolesByUserIdAsync(string userId, bool trackChanges, CancellationToken cancellationToken = default);
}
