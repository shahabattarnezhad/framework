using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Authentication;

namespace Service.Contracts.Base;

public interface IServiceManager
{
    IAuthenticationService AuthenticationService { get; }

    ICompanyService CompanyService { get; }

    IEmployeeService EmployeeService { get; }
}
