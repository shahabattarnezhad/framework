using Shared.DTOs.Sample.Employee;

namespace Service.Contracts.Interfaces;

public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetAll(Guid companyId, bool trackChanges);

    EmployeeDto Get(Guid companyId, Guid id, bool trackChanges);

    EmployeeDto CreateEmployeeForCompany(Guid companyId,
                                         EmployeeForCreationDto employeeForCreation,
                                         bool trackChanges);

    void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);

    void UpdateEmployeeForCompany(Guid companyId,
                                  Guid id,
                                  EmployeeForUpdateDto entityForUpdate,
                                  bool companyTrackChanges,
                                  bool employeeTrackChanges);
}
