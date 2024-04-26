using AutoMapper;
using Contracts.Base;
using Contracts.DataShaping;
using Contracts.Logging;
using Entities.Exceptions.General;
using Entities.Exceptions.Sample.Company;
using Entities.Exceptions.Sample.Employee;
using Entities.Models.Base;
using Entities.Models.Sample;
using Service.Contracts.Interfaces;
using Service.DataShaping;
using Shared.DTOs.Sample.Employee;
using Shared.RequestFeatures.Base;
using Shared.RequestFeatures.Sample;
using System.Dynamic;

namespace Service.Services.Sample;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<EmployeeDto> _dataShaper;

    public EmployeeService(IRepositoryManager repository,
                           ILoggerManager logger,
                           IMapper mapper,
                           IDataShaper<EmployeeDto> dataShaper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
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


    public async Task<(IEnumerable<Entity> employees, MetaData metaData)>
        GetAllAsync(Guid companyId,
                    EmployeeParameters employeeParameters,
                    bool trackChanges)
    {
        if (!employeeParameters.ValidAgeRange) 
            throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExists(companyId, trackChanges);

        var entitiesWithMetaData =
            await _repository.Employee.GetAllAsync(companyId,
                                                   employeeParameters,
                                                   trackChanges);

        var entitiesDto =
            _mapper.Map<IEnumerable<EmployeeDto>>(entitiesWithMetaData);

        var shapedData = _dataShaper.ShapeData(entitiesDto,
            employeeParameters.Fields!);

        return (employees: shapedData, metaData: entitiesWithMetaData.MetaData)!;
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
