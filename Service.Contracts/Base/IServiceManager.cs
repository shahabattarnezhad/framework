using Service.Contracts.Interfaces;

namespace Service.Contracts.Base;

public interface IServiceManager
{
    ICompanyService CompanyService { get; }

    IEmployeeService EmployeeService { get; }
}
