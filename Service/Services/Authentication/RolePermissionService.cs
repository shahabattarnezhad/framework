using AutoMapper;
using Contracts.Base;
using Entities.Exceptions.Authentication;
using Entities.Exceptions.General;
using Service.Contracts.Interfaces.Authentication;
using Shared.DTOs.Authentication;

namespace Service.Services.Authentication;

internal sealed class RolePermissionService : IRolePermissionService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public RolePermissionService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PermissionDto>> GetPermissionsByRoleIdAsync
        (string roleId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var permissions = 
            await _repository.RolePermission.GetPermissionsByRoleIdAsync(roleId, trackChanges, cancellationToken);

        return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
    }

    public async Task AssignPermissionToRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        var role = await _repository.Role.GetByIdAsync(roleId, true, cancellationToken);
        if (role == null)
            throw new RoleNotFoundByIdException(roleId);

        var permission = await _repository.Permission.GetByIdAsync(permissionId, false, cancellationToken);

        if (permission == null)
            throw new EntityNotFoundException(permissionId);

        var exists = await _repository.RolePermission.ExistsAsync(roleId, permissionId, cancellationToken);
        if (exists)
            return;

        await _repository.RolePermission.AssignPermissionToRoleAsync(roleId, permissionId, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task RemovePermissionFromRoleAsync(string roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        await _repository.RolePermission.RemovePermissionFromRoleAsync(roleId, permissionId, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }
}
