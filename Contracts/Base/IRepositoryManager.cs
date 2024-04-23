using Contracts.Interfaces.Sample;

namespace Contracts.Base;

public interface IRepositoryManager
{
    ICompanyRepository Company { get; }

    IEmployeeRepository Employee { get; }

    Task SaveAsync();
}
