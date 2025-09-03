using Entities.Models.Authentication;
using Shared.RequestFeatures.Authentication;
using Shared.RequestFeatures.Base;

namespace Contracts.Interfaces.Authentication;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default);

    Task<PagedList<User>> GetAllAsync
        (UserParameters entityParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<PagedList<User>> GetAllWithRolesAsync
        (UserParameters entityParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(string id, bool trackChanges, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string email, bool trackChanges, CancellationToken cancellationToken = default);

    Task<User?> GetByUserNameAsync(string userName, bool trackChanges, CancellationToken cancellationToken = default);

    Task<bool> CheckIfUserExistsAsync(string userId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
