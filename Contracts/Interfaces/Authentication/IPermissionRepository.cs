using Entities.Models.Authentication;

namespace Contracts.Interfaces.Authentication;

public interface IPermissionRepository
{
    Task<IEnumerable<Permission>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default);

    Task<Permission?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);
}
