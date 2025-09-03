using Entities.Models.Authentication;
using Shared.RequestFeatures.Authentication;
using Shared.RequestFeatures.Base;

namespace Contracts.Interfaces.Authentication;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default);

    Task<PagedList<Role>> GetAllAsync
        (RoleParameters entityParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<Role?> GetByIdAsync(string id, bool trackChanges, CancellationToken cancellationToken = default);

    Task<Role?> GetByRoleNameAsync(string roleName, bool trackChanges, CancellationToken cancellationToken = default);
}
