using Entities.Models.Sample;

namespace Contracts.Interfaces.Sample;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync(Guid companyId, bool trackChanges);

    Task<Employee>? GetAsync(Guid companyId, Guid id, bool trackChanges);

    void CreateEmployeeForCompany(Guid companyId, Employee employee);

    void DeleteEntity(Employee employee);
}
