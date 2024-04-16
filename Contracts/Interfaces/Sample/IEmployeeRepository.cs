using Entities.Models.Sample;

namespace Contracts.Interfaces.Sample;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetAll(Guid companyId, bool trackChanges);

    Employee? Get(Guid companyId, Guid id, bool trackChanges);
}
