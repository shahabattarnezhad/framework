using Entities.LinkModels.Authentication;
using Entities.LinkModels.Base;
using Entities.Models.Authentication;
using Shared.DTOs.Authentication;
using Shared.DTOs.Sample.Company;
using Shared.RequestFeatures.Base;

namespace Service.Contracts.Interfaces.Authentication;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default);

    Task<(LinkResponse linkResponse, MetaData metaData)> GetAllAsync
        (UserLinkParameters linkParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<(LinkResponse linkResponse, MetaData metaData)> GetAllWithRolesAsync
        (UserLinkParameters linkParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<UserDto?> GetByIdAsync(string id, bool trackChanges, CancellationToken cancellationToken = default);

    Task<UserDto?> GetByEmailAsync(string email, bool trackChanges, CancellationToken cancellationToken = default);

    Task<UserDto?> GetByUserNameAsync(string userName, bool trackChanges, CancellationToken cancellationToken = default);

    Task<(UserForPatchDto entityToPatch, User entity)> GetUserForPatchAsync
        (string userId, bool trackChanges, CancellationToken cancellationToken = default);

    Task SaveChangesForPatchAsync(UserForPatchDto entityToPatch, User entity, CancellationToken cancellationToken = default);

    Task<bool> AnyUserExistsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task UpdateAsync
        (string userId, UserForUpdationDto userForUpdate, bool trackChanges, CancellationToken cancellationToken = default);
}
