using AutoMapper;
using Contracts.Base;
using Contracts.DataShaping;
using Contracts.Links.Sample;
using Contracts.Logging;
using Entities.Exceptions.General;
using Entities.Exceptions.Sample.Company;
using Entities.LinkModels.Base;
using Entities.LinkModels.Sample;
using Entities.Models.Sample;
using Service.Contracts.Interfaces;
using Shared.DTOs.Sample.Company;
using Shared.RequestFeatures.Base;

namespace Service.Services.Sample;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<CompanyDto, Guid> _dataShaper;
    private readonly ICompanyLinks _companyLinks;

    public CompanyService(IRepositoryManager repository,
                          ILoggerManager logger,
                          IMapper mapper,
                          IDataShaper<CompanyDto, Guid> dataShaper,
                          ICompanyLinks companyLinks)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
        _companyLinks = companyLinks;
    }


    public async Task<IEnumerable<CompanyDto>> GetAllAsync(bool trackChanges)
    {
        var entities =
                await _repository.Company.GetAllAsync(trackChanges);

        var entitiesDto =
            _mapper.Map<IEnumerable<CompanyDto>>(entities);

        return entitiesDto;
    }


    public async Task<(LinkResponse linkResponse, MetaData metaData)> 
        GetAllAsync(CompanyLinkParameters linkParameters, bool trackChanges)
    {
        var entitiesWithMetaData =await _repository.Company
            .GetAllAsync(linkParameters.CompanyParameters, trackChanges);

        var entitiesDto =
        _mapper.Map<IEnumerable<CompanyDto>>(entitiesWithMetaData);

        var links = _companyLinks.TryGenerateLinks(entitiesDto,
            linkParameters.CompanyParameters.Fields!,
            linkParameters.Context);

        return (linkResponse: links, metaData: entitiesWithMetaData.MetaData!);
    }


    public async Task<CompanyDto>? GetDuplicateNameAsync(string entityName, bool trackChanges)
    {
        var entity =
            await GetCompanyByNameAndCheckIfItExists(entityName, trackChanges)!;

        var entityDto =
            _mapper.Map<CompanyDto>(entity);

        return entityDto;
    }


    public async Task<CompanyDto>? GetAsync(Guid entityId, bool trackChanges)
    {
        var entity =
            await GetCompanyAndCheckIfItExists(entityId, trackChanges);

        var entityDto =
            _mapper.Map<CompanyDto>(entity);

        return entityDto;
    }


    public async Task<CompanyDto> CreateAsync(CompanyForCreationDto entityForCreation)
    {
        await GetDuplicateNameAsync(entityForCreation.Name!, false)!;

        var entity =
            _mapper.Map<Company>(entityForCreation);

        _repository.Company.CreateEntity(entity);
        await _repository.SaveAsync();

        var entityToReturn =
            _mapper.Map<CompanyDto>(entity);

        return entityToReturn;
    }


    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var entities =
            await _repository.Company.GetByIdsAsync(ids, trackChanges);

        if (ids.Count() != entities.Count())
            throw new CollectionByIdsBadRequestException();

        var entitiesToReturn =
            _mapper.Map<IEnumerable<CompanyDto>>(entities);

        return entitiesToReturn;
    }


    public async Task<(IEnumerable<CompanyDto> entities, string ids)>
        CreateEntityCollectionAsync(IEnumerable<CompanyForCreationDto> entityCollection)
    {
        if (entityCollection is null)
            throw new CompanyCollectionBadRequest();

        var entities =
            _mapper.Map<IEnumerable<Company>>(entityCollection);

        foreach (var entity in entities)
        {
            _repository.Company.CreateEntity(entity);
        }

        await _repository.SaveAsync();

        var entityCollectionToReturn =
            _mapper.Map<IEnumerable<CompanyDto>>(entities);

        var ids =
            string.Join(",", entityCollectionToReturn
                  .Select(c => c.Id));

        return (entities: entityCollectionToReturn, ids: ids);
    }


    public async Task DeleteAsync(Guid entityId, bool trackChanges)
    {
        var entity =
            await GetCompanyAndCheckIfItExists(entityId, trackChanges);

        await GetCompanyAndCheckIfItHasAnyChild(entityId, trackChanges);

        _repository.Company.DeleteEntity(entity);
        await _repository.SaveAsync();
    }


    public async Task UpdateAsync(Guid entityId,
                       CompanyForUpdateDto entityForUpdate,
                       bool trackChanges)
    {
        var entity =
            await GetCompanyAndCheckIfItExists(entityId, trackChanges);

        _mapper.Map(entityForUpdate, entity);

        await _repository.SaveAsync();
    }


    private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
    {
        var company =
            await _repository.Company.GetAsync(id, trackChanges)!;

        if (company is null)
            throw new CompanyNotFoundException(id);

        return company;
    }


    private async Task<Company> GetCompanyByNameAndCheckIfItExists(string entityName,
                                                                   bool trackChanges)
    {
        var company =
            await _repository.Company.GetDuplicateNameAsync(entityName, trackChanges)!;

        if (company is not null)
            throw new CompanyDuplicatedNameException();

        return company!;
    }


    private async Task<IEnumerable<Employee>> GetCompanyAndCheckIfItHasAnyChild(Guid id, bool trackChanges)
    {
        var hasEntityAnyChild =
                await _repository.Employee.GetAllAsync(id, trackChanges);

        if (hasEntityAnyChild.Count() > 0)
            throw new EntityCannotBeDeletedException();

        return hasEntityAnyChild;
    }
}
