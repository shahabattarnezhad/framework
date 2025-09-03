using AutoMapper;
using Contracts.Base;
using Contracts.DataShaping;
using Contracts.Links.Authentication;
using Contracts.Logging;
using Entities.Exceptions.Authentication;
using Entities.LinkModels.Authentication;
using Entities.LinkModels.Base;
using Entities.Models.Authentication;
using Service.Contracts.Interfaces.Authentication;
using Shared.DTOs.Authentication;
using Shared.RequestFeatures.Base;

namespace Service.Services.Authentication;

internal sealed class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<UserForDisplayDto, string> _dataShaper;
    private readonly IUserLinks _userLinks;

    public UserService(IRepositoryManager repository,
                          ILoggerManager logger,
                          IMapper mapper,
                          IDataShaper<UserForDisplayDto, string> dataShaper,
                          IUserLinks userLinks)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
        _userLinks = userLinks;
    }


    public async Task<IEnumerable<UserDto>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.User.GetAllAsync(trackChanges, cancellationToken);

        var entitiesDto = _mapper.Map<IEnumerable<UserDto>>(entities);

        return entitiesDto;
    }

    public async Task<(LinkResponse linkResponse, MetaData metaData)>
        GetAllAsync(UserLinkParameters linkParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entitiesWithMetaData =
            await _repository.User.GetAllAsync(linkParameters.UserParameters, trackChanges, cancellationToken);

        var entitiesDto = _mapper.Map<IEnumerable<UserForDisplayDto>>(entitiesWithMetaData);

        var links = _userLinks
            .TryGenerateLinks(entitiesDto, linkParameters.UserParameters.Fields!, linkParameters.Context);

        return (linkResponse: links, metaData: entitiesWithMetaData.MetaData!);
    }

    public async Task<(LinkResponse linkResponse, MetaData metaData)>
        GetAllWithRolesAsync(UserLinkParameters linkParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entitiesWithMetaData =
            await _repository.User.GetAllWithRolesAsync(linkParameters.UserParameters, trackChanges, cancellationToken);

        var entitiesDto = _mapper.Map<IEnumerable<UserForDisplayDto>>(entitiesWithMetaData);

        var links = _userLinks
            .TryGenerateLinks(entitiesDto, linkParameters.UserParameters.Fields!, linkParameters.Context);

        return (linkResponse: links, metaData: entitiesWithMetaData.MetaData!);
    }

    public async Task<UserDto?> GetByEmailAsync(string email, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var user = await _repository.User.GetByEmailAsync(email, trackChanges, cancellationToken)!;

        if (user is null)
            throw new UserNotFoundByEmailException(email);

        var entityDto = _mapper.Map<UserDto>(user);

        return entityDto;
    }

    public async Task<UserDto?> GetByIdAsync(string id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var user = await _repository.User.GetByIdAsync(id, trackChanges, cancellationToken)!;

        if (user is null)
            throw new UserNotFoundByIdException(id);

        var entityDto = _mapper.Map<UserDto>(user);

        var roles = await _repository.UserRole.GetRolesByUserIdAsync(id, trackChanges, cancellationToken);

        entityDto.Roles = roles.Select(r => r.Name!).ToList();

        return entityDto;
    }

    public async Task<UserDto?> GetByUserNameAsync(string userName, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var user = await _repository.User.GetByUserNameAsync(userName, trackChanges, cancellationToken)!;

        if (user is null)
            throw new UserNotFoundByUserNameException(userName);

        var entityDto = _mapper.Map<UserDto>(user);
        return entityDto;
    }

    public async Task<(UserForPatchDto entityToPatch, User entity)> GetUserForPatchAsync
        (string userId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.User.GetByIdAsync(userId, trackChanges, cancellationToken)!;

        var childToPatch = _mapper.Map<UserForPatchDto>(entity);

        return (childToPatch, entity);
    }

    public async Task SaveChangesForPatchAsync(UserForPatchDto entityToPatch, User entity, CancellationToken cancellationToken = default)
    {
        _mapper.Map(entityToPatch, entity);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<bool> AnyUserExistsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _repository.User.ExistsAsync(trackChanges, cancellationToken);
    }

    public async Task UpdateAsync
        (string userId, UserForUpdationDto userForUpdate, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await GetUserAndCheckIfItExists(userId, trackChanges);

        _mapper.Map(userForUpdate, entity);

        await _repository.SaveAsync(cancellationToken);
    }


    private async Task<User> GetUserAndCheckIfItExists(string id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var user =
            await _repository.User.GetByIdAsync(id, trackChanges, cancellationToken)!;

        if (user is null)
            throw new UserNotFoundByIdException(id);

        return user;
    }
}
