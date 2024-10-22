﻿using AutoMapper;
using Contracts.Base;
using Contracts.DataShaping;
using Contracts.Links.Sample;
using Contracts.Logging;
using Entities.Exceptions.General;
using Entities.Exceptions.Sample.Company;
using Entities.Exceptions.Sample.Employee;
using Entities.Models.LinkModels.Base;
using Entities.Models.LinkModels.Sample;
using Entities.Models.Sample;
using Service.Contracts.Interfaces;
using Shared.DTOs.Sample.Employee;
using Shared.RequestFeatures.Base;

namespace Service.Services.Sample;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<EmployeeDto> _dataShaper;
    private readonly IEmployeeLinks _employeeLinks;

    public EmployeeService(IRepositoryManager repository,
                           ILoggerManager logger,
                           IMapper mapper,
                           IDataShaper<EmployeeDto> dataShaper,
                           IEmployeeLinks employeeLinks)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
        _employeeLinks = employeeLinks;
    }

    public async Task<EmployeeDto> GetAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var entityFromDb =
            await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

        var entityDto =
            _mapper.Map<EmployeeDto>(entityFromDb);

        return entityDto;
    }


    public async Task<IEnumerable<EmployeeDto>> GetAllAsync(Guid companyId,
                                                            bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var entitiesFromDb =
            await _repository.Employee.GetAllAsync(companyId, trackChanges);

        var entitiesDto =
            _mapper.Map<IEnumerable<EmployeeDto>>(entitiesFromDb);

        return entitiesDto;
    }


    public async Task<(LinkResponse linkResponse, MetaData metaData)>
        GetAllAsync(Guid companyId,
                    EmployeeLinkParameters linkParameters,
                    bool trackChanges)
    {
        if (!linkParameters.EmployeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExists(companyId, trackChanges);

        var entitiesWithMetaData =
            await _repository.Employee.GetAllAsync(companyId,
                                    linkParameters.EmployeeParameters,
                                                   trackChanges);

        var entitiesDto =
            _mapper.Map<IEnumerable<EmployeeDto>>(entitiesWithMetaData);

        var links = 
            _employeeLinks.TryGenerateLinks(entitiesDto,
                                     linkParameters.EmployeeParameters.Fields!,
                                            companyId,
                                  linkParameters.Context);

        return (linkResponse: links, metaData: entitiesWithMetaData.MetaData!);
    }


    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId,
                                                EmployeeForCreationDto employeeForCreation,
                                                bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var entityForSave =
            _mapper.Map<Employee>(employeeForCreation);

        _repository.Employee.CreateEmployeeForCompany(companyId, entityForSave);
        await _repository.SaveAsync();

        var entityToReturn =
            _mapper.Map<EmployeeDto>(entityForSave);

        return entityToReturn;
    }


    public async Task DeleteEmployeeForCompanyAsync(Guid companyId,
                                                    Guid id,
                                                    bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeForCompany =
            await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

        _repository.Employee.DeleteEntity(employeeForCompany);
        await _repository.SaveAsync();
    }


    public async Task UpdateEmployeeForCompanyAsync(Guid companyId,
                                                    Guid id,
                                                    EmployeeForUpdateDto entityForUpdate,
                                                    bool companyTrackChanges,
                                                    bool employeeTrackChanges)
    {
        await CheckIfCompanyExists(companyId, companyTrackChanges);

        var childEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId,
            id,
            employeeTrackChanges);

        _mapper.Map(entityForUpdate, childEntity);

        await _repository.SaveAsync();
    }


    public async Task<(EmployeeForUpdateDto entityToPatch, Employee entity)>
        GetEmployeeForPatchAsync(Guid companyId,
                                 Guid id,
                                 bool companyTrackChanges,
                                 bool employeeTrackChanges)
    {
        await CheckIfCompanyExists(companyId, companyTrackChanges);

        var childEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId,
            id,
            employeeTrackChanges);

        var childToPatch =
            _mapper.Map<EmployeeForUpdateDto>(childEntity);

        return (childToPatch, childEntity);
    }


    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto entityToPatch,
                                               Employee entity)
    {
        _mapper.Map(entityToPatch, entity);
        await _repository.SaveAsync();
    }


    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
    {
        var entity =
            await _repository.Company.GetAsync(companyId, trackChanges)!;

        if (entity is null)
            throw new CompanyNotFoundException(companyId);
    }


    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId,
                                                                         Guid id,
                                                                         bool trackChanges)
    {
        var entityDb =
            await _repository.Employee.GetAsync(companyId, id, trackChanges)!;

        if (entityDb is null)
            throw new EmployeeNotFoundException(id);

        return entityDb;
    }
}
