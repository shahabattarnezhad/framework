using Entities.Models.Sample;
using Shared.RequestFeatures.Base;
using Shared.RequestFeatures.Sample;

namespace Contracts.Interfaces.Sample;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetAllAsync(Guid companyId,
                                          EmployeeParameters employeeParameters,
                                          bool trackChanges);

    Task<IEnumerable<Employee>> GetAllAsync(Guid companyId,
                                            bool trackChanges);

    Task<Employee>? GetAsync(Guid companyId, Guid id, bool trackChanges);

    void CreateEmployeeForCompany(Guid companyId, Employee employee);

    void DeleteEntity(Employee employee);
}
