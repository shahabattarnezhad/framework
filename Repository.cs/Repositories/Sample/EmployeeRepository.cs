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
}
