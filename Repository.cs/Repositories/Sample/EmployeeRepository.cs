using Contracts.Interfaces.Sample;
using Entities.Models.Sample;
using Repository.Base;
using Repository.Data;

namespace Repository.Repositories.Sample;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }


    public IEnumerable<Employee> GetAll(Guid companyId, bool trackChanges) =>
        FindByCondition(entity =>
                       entity.CompanyId.Equals(companyId), trackChanges).
                       OrderBy(entity => entity.FullName).
                       ToList();


    public Employee? Get(Guid companyId, Guid id, bool trackChanges) =>
                     FindByCondition(entity => 
                     entity.CompanyId.Equals(companyId) && 
                     entity.Id.Equals(id), trackChanges).
                     SingleOrDefault();


    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;

        Create(employee);
    }


    public void DeleteEntity(Employee employee) => Delete(employee);
}
