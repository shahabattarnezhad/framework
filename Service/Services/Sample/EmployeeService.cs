using AutoMapper;
using Contracts.Base;
using Contracts.Logging;
using Entities.Exceptions.Sample.Company;
using Entities.Exceptions.Sample.Employee;
using Entities.Models.Sample;
using Service.Contracts.Interfaces;
using Shared.DTOs.Sample.Employee;

namespace Service.Services.Sample;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repository,
                           ILoggerManager logger,
                           IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public EmployeeDto Get(Guid companyId, Guid id, bool trackChanges)
    {
        var entity =
            _repository.Company.Get(companyId, trackChanges);

        if (entity is null)
            throw new CompanyNotFoundException(companyId);

        var entityFromDb =
            _repository.Employee.Get(companyId, id, trackChanges);

        if (entityFromDb is null)
            throw new EmployeeNotFoundException(id);

        var entityDto =
            _mapper.Map<EmployeeDto>(entityFromDb);

        return entityDto;
    }


    public IEnumerable<EmployeeDto> GetAll(Guid companyId, bool trackChanges)
    {
        var entity =
            _repository.Company.Get(companyId, trackChanges);

        if (entity is null)
            throw new CompanyNotFoundException(companyId);

        var entitiesFromDb =
            _repository.Employee.GetAll(companyId, trackChanges);

        var entitiesDto =
            _mapper.Map<IEnumerable<EmployeeDto>>(entitiesFromDb);

        return entitiesDto;
    }


    public EmployeeDto CreateEmployeeForCompany(Guid companyId,
                                                EmployeeForCreationDto employeeForCreation,
                                                bool trackChanges)
    {
        var entity =
            _repository.Company.Get(companyId, trackChanges);

        if (entity is null)
            throw new CompanyNotFoundException(companyId);

        var entityForSave =
            _mapper.Map<Employee>(employeeForCreation);

        _repository.Employee.CreateEmployeeForCompany(companyId, entityForSave);
        _repository.Save();

        var entityToReturn =
            _mapper.Map<EmployeeDto>(entityForSave);

        return entityToReturn;
    }


    public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
    {
        var entity =
            _repository.Company.Get(companyId, trackChanges);

        if (entity is null)
            throw new CompanyNotFoundException(companyId);

        var employeeForCompany =
            _repository.Employee.Get(companyId, id, trackChanges);

        if (employeeForCompany is null)
            throw new EmployeeNotFoundException(id);

        _repository.Employee.DeleteEntity(employeeForCompany);
        _repository.Save();
    }


    public void UpdateEmployeeForCompany(Guid companyId,
                                         Guid id,
                                         EmployeeForUpdateDto entityForUpdate,
                                         bool companyTrackChanges,
                                         bool employeeTrackChanges)
    {
        var entity =
            _repository.Company.Get(companyId, companyTrackChanges);

        if (entity is null)
            throw new CompanyNotFoundException(companyId);

        var childEntity =
            _repository.Employee.Get(companyId, id, employeeTrackChanges);

        if (childEntity is null)
            throw new EmployeeNotFoundException(id);

        _mapper.Map(entityForUpdate, childEntity);

        _repository.Save();
    }


    public (EmployeeForUpdateDto entityToPatch, Employee entity) GetEmployeeForPatch
                                                                (Guid companyId,
                                                                 Guid id,
                                                                 bool companyTrackChanges,
                                                                 bool employeeTrackChanges)
    {
        var parentEntityFromDb =
            _repository.Company.Get(companyId, companyTrackChanges);

        if (parentEntityFromDb is null)
            throw new CompanyNotFoundException(companyId);

        var childEntity =
            _repository.Employee.Get(companyId, id, employeeTrackChanges);

        if (childEntity is null)
            throw new EmployeeNotFoundException(companyId);

        var childToPatch =
            _mapper.Map<EmployeeForUpdateDto>(childEntity);

        return (childToPatch, childEntity);
    }


    public void SaveChangesForPatch(EmployeeForUpdateDto entityToPatch, Employee entity)
    {
        _mapper.Map(entityToPatch, entity);
        _repository.Save();
    }
}
