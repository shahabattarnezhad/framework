using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Authentication;

namespace Service.Contracts.Base;

public interface IServiceManager
{
    // Sample:
    ICompanyService CompanyService { get; }
    IEmployeeService EmployeeService { get; }

    // Authentication:
    IAuthenticationService AuthenticationService { get; }
    IUserService UserService { get; }
    IRoleService RoleService { get; }
    IRolePermissionService RolePermissionService { get; }
    IUserRoleService UserRoleService { get; }
}
