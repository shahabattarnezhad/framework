using AutoMapper;
using Contracts.Base;
using Contracts.DataShaping;
using Contracts.Links.Authentication;
using Contracts.Logging;
using Entities.Exceptions.Authentication;
using Entities.LinkModels.Authentication;
using Entities.LinkModels.Base;
using Service.Contracts.Interfaces.Authentication;
using Shared.DTOs.Authentication;
using Shared.RequestFeatures.Base;

namespace Service.Services.Authentication;

internal sealed class RoleService : IRoleService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<RoleDto, string> _dataShaper;
    private readonly IRoleLinks _roleLinks;

    public RoleService(IRepositoryManager repository,
                          ILoggerManager logger,
                          IMapper mapper,
                          IDataShaper<RoleDto, string> dataShaper,
                          IRoleLinks roleLinks)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
        _roleLinks = roleLinks;
    }


    public async Task<IEnumerable<RoleDto>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.Role.GetAllAsync(trackChanges, cancellationToken);

        var entitiesDto = _mapper.Map<IEnumerable<RoleDto>>(entities);

        return entitiesDto;
    }

    public async Task<(LinkResponse linkResponse, MetaData metaData)>
        GetAllAsync(RoleLinkParameters linkParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entitiesWithMetaData =
            await _repository.Role.GetAllAsync(linkParameters.RoleParameters, trackChanges, cancellationToken);

        var entitiesDto = _mapper.Map<IEnumerable<RoleDto>>(entitiesWithMetaData);

        var links = _roleLinks
            .TryGenerateLinks(entitiesDto, linkParameters.RoleParameters.Fields!, linkParameters.Context);

        return (linkResponse: links, metaData: entitiesWithMetaData.MetaData!);
    }

    public async Task<RoleDto?> GetByIdAsync(string id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var role = await _repository.Role.GetByIdAsync(id, trackChanges, cancellationToken)!;

        if (role is null)
            throw new RoleNotFoundByIdException(id);

        var entityDto = _mapper.Map<RoleDto>(role);

        return entityDto;
    }

    public async Task<RoleDto?> GetByRoleNameAsync(string roleName, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var role = await _repository.Role.GetByRoleNameAsync(roleName, trackChanges, cancellationToken)!;

        if (role is null)
            throw new RoleNotFoundByNameException(roleName);

        var entityDto = _mapper.Map<RoleDto>(role);
        return entityDto;
    }
}
