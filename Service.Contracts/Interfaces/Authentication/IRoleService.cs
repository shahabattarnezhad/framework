using Entities.LinkModels.Authentication;
using Entities.LinkModels.Base;
using Shared.DTOs.Authentication;
using Shared.RequestFeatures.Base;

namespace Service.Contracts.Interfaces.Authentication;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default);

    Task<(LinkResponse linkResponse, MetaData metaData)> GetAllAsync
        (RoleLinkParameters linkParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<RoleDto?> GetByIdAsync(string id, bool trackChanges, CancellationToken cancellationToken = default);

    Task<RoleDto?> GetByRoleNameAsync(string roleName, bool trackChanges, CancellationToken cancellationToken = default);
}
