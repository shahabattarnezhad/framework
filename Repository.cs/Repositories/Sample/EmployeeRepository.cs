using Contracts.Interfaces.Sample;
using Entities.Models.Sample;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;

namespace Repository.Repositories.Sample;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }


    public async Task<IEnumerable<Employee>> GetAllAsync(Guid companyId, bool trackChanges) =>
                       await FindByCondition(entity =>
                       entity.CompanyId.Equals(companyId), trackChanges).
                       OrderBy(entity => entity.FullName).
                       ToListAsync();


    public async Task<Employee>? GetAsync(Guid companyId, Guid id, bool trackChanges) =>
                     await FindByCondition(entity => 
                     entity.CompanyId.Equals(companyId) && 
                     entity.Id.Equals(id), trackChanges).
                     SingleOrDefaultAsync();


    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;

        Create(employee);
    }


    public void DeleteEntity(Employee employee) => Delete(employee);
}
