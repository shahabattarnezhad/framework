using Contracts.Interfaces.Sample;
using Entities.Models.Sample;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Repository.Extensions.Sample;
using Shared.RequestFeatures.Base;
using Shared.RequestFeatures.Sample;

namespace Repository.Repositories.Sample;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }


    public async Task<IEnumerable<Employee>> GetAllAsync(Guid companyId,
        bool trackChanges) =>
        await FindByCondition(entity =>
        entity.CompanyId.Equals(companyId), trackChanges).
        OrderBy(entity => entity.FullName).
        ToListAsync();


    public async Task<PagedList<Employee>> GetAllAsync(Guid companyId,
        EmployeeParameters employeeParameters, bool trackChanges)
    {
        var entities =
            await FindByCondition(entity =>
                                entity.CompanyId.Equals(companyId), trackChanges).
                                FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge).
                                Search(employeeParameters.SearchTerm!).
                                OrderBy(entity => entity.FullName).
                                Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize).
                                Take(employeeParameters.PageSize).
                                ToListAsync();

        var count = await FindByCondition(entity => 
        entity.CompanyId.Equals(companyId), trackChanges).CountAsync();

        return new PagedList<Employee>(entities,
                                             count,
                                             employeeParameters.PageNumber,
                                             employeeParameters.PageSize);
    }


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
