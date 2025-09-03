using AutoMapper;
using Contracts.Base;
using Service.Contracts.Interfaces.Authentication;
using Shared.DTOs.Authentication;

namespace Service.Services.Authentication;

internal sealed class UserRoleService : IUserRoleService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public UserRoleService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task AssignRoleToUserAsync(string userId, string roleId, CancellationToken cancellationToken = default)
    {
        await _repository.UserRole.AssignRoleToUserAsync(userId, roleId, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task RemoveRoleFromUserAsync(string userId, string roleId, CancellationToken cancellationToken = default)
    {
        await _repository.UserRole.RemoveRoleFromUserAsync(userId, roleId, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<List<RoleDto>> GetRolesByUserIdAsync(string userId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var roles = await _repository.UserRole.GetRolesByUserIdAsync(userId, trackChanges, cancellationToken);
        return _mapper.Map<List<RoleDto>>(roles);
    }
}
