using AutoMapper;
using Contracts.Base;
using Contracts.DataShaping;
using Contracts.Links.Authentication;
using Contracts.Links.Sample;
using Contracts.Logging;
using Entities.ConfigurationModels;
using Entities.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Authentication;
using Service.Services.Authentication;
using Service.Services.Sample;
using Shared.DTOs.Authentication;
using Shared.DTOs.Sample.Company;
using Shared.DTOs.Sample.Employee;

namespace Service.Base;

public sealed class ServiceManager : IServiceManager
{
    // Sample:
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;

    // Authentication:
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IRoleService> _roleService;
    private readonly Lazy<IUserRoleService> _userRoleService;
    private readonly Lazy<IRolePermissionService> _rolePermissionService;


    public ServiceManager(IRepositoryManager repositoryManager,
                          ILoggerManager logger,
                          IMapper mapper,
                          UserManager<User> userManager,
                          IUserProfileImageService profileImageService,
                          IOptions<JwtConfiguration> configuration,
                          IDataShaper<CompanyDto, Guid> companyDataShaper,
                          IDataShaper<EmployeeDto, Guid> employeeDataShaper,
                          IDataShaper<UserForDisplayDto, string> userDataShaper,
                          IDataShaper<RoleDto, string> roleDataShaper,
                          ICompanyLinks companyLinks,
                          IEmployeeLinks employeeLinks,
                          IUserLinks userLinks,
                          IRoleLinks roleLinks)
    {
        _companyService = new Lazy<ICompanyService>(() =>
        new CompanyService(repositoryManager, logger, mapper, companyDataShaper, companyLinks));

        _employeeService = new Lazy<IEmployeeService>(() =>
        new EmployeeService(repositoryManager, logger, mapper, employeeDataShaper, employeeLinks));


        // Authentication:
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService
        (logger, mapper, userManager, configuration, profileImageService, repositoryManager));

        _userService = new Lazy<IUserService>
            (() => new UserService(repositoryManager, logger, mapper, userDataShaper, userLinks));

        _roleService = new Lazy<IRoleService>
            (() => new RoleService(repositoryManager, logger, mapper, roleDataShaper, roleLinks));

        _rolePermissionService = new Lazy<IRolePermissionService>
            (() => new RolePermissionService(repositoryManager, mapper));

        _userRoleService = new Lazy<IUserRoleService>
            (() => new UserRoleService(repositoryManager, mapper));
    }

    // Sample:
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;


    // Authentication:
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public IUserService UserService => _userService.Value;
    public IRoleService RoleService => _roleService.Value;
    public IUserRoleService UserRoleService => _userRoleService.Value;
    public IRolePermissionService RolePermissionService => _rolePermissionService.Value;
}
