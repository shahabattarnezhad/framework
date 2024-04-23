using AutoMapper;
using Contracts.Base;
using Contracts.Logging;
using Entities.Exceptions.General;
using Entities.Exceptions.Sample.Company;
using Entities.Models.Sample;
using Service.Contracts.Interfaces;
using Shared.DTOs.Sample.Company;

namespace Service.Services.Sample;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repository,
                          ILoggerManager logger,
                          IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public IEnumerable<CompanyDto> GetAll(bool trackChanges)
    {
        var entities =
                _repository.Company.GetAll(trackChanges);

        var entitiesDto =
            _mapper.Map<IEnumerable<CompanyDto>>(entities);

        return entitiesDto;
    }


    public CompanyDto? GetDuplicateName(string entityName, bool trackChanges)
    {
        var entity =
            _repository.Company.GetDuplicateName(entityName, trackChanges);

        if (entity is not null)
            throw new CompanyDuplicatedNameException();

        var entityDto =
            _mapper.Map<CompanyDto>(entity);

        return entityDto;
    }


    public CompanyDto Get(Guid entityId, bool trackChanges)
    {
        var entity =
            _repository.Company.Get(entityId, trackChanges);

        if (entity is null)
            throw new CompanyNotFoundException(entityId);

        var entityDto =
            _mapper.Map<CompanyDto>(entity);

        return entityDto;
    }


    public CompanyDto Create(CompanyForCreationDto entityForCreation)
    {
        GetDuplicateName(entityForCreation.Name!, false);

        var entity =
            _mapper.Map<Company>(entityForCreation);

        _repository.Company.CreateEntity(entity);
        _repository.Save();

        var entityToReturn =
            _mapper.Map<CompanyDto>(entity);

        return entityToReturn;
    }


    public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var entities =
            _repository.Company.GetByIds(ids, trackChanges);

        if (ids.Count() != entities.Count())
            throw new CollectionByIdsBadRequestException();

        var entitiesToReturn =
            _mapper.Map<IEnumerable<CompanyDto>>(entities);

        return entitiesToReturn;
    }


    public (IEnumerable<CompanyDto> entities, string ids)
        CreateEntityCollection(IEnumerable<CompanyForCreationDto> entityCollection)
    {
        if (entityCollection is null)
            throw new CompanyCollectionBadRequest();

        var entities =
            _mapper.Map<IEnumerable<Company>>(entityCollection);

        foreach (var entity in entities)
        {
            _repository.Company.CreateEntity(entity);
        }

        _repository.Save();

        var entityCollectionToReturn =
            _mapper.Map<IEnumerable<CompanyDto>>(entities);

        var ids =
            string.Join(",", entityCollectionToReturn
                  .Select(c => c.Id));

        return (companies: entityCollectionToReturn, ids: ids);
    }


    public void Delete(Guid entityId, bool trackChanges)
    {
        var entity =
            _repository.Company.Get(entityId, trackChanges);

        if (entity is null)
            throw new CompanyNotFoundException(entityId);

        var hasEntityAnyChild =
                _repository.Employee.GetAll(entityId, trackChanges);

        if (hasEntityAnyChild.Count() > 0)
        {
            throw new EntityCannotBeDeletedException();
        }
        else
        {
            _repository.Company.DeleteEntity(entity);
            _repository.Save();
        }
    }


    public void Update(Guid entityId,
                       CompanyForUpdateDto entityForUpdate,
                       bool trackChanges)
    {
        var entity =
            _repository.Company.Get(entityId, trackChanges); 
        
        if (entity is null) 
            throw new CompanyNotFoundException(entityId); 
        
        _mapper.Map(entityForUpdate, entity);
        
        _repository.Save();
    }
}
