using AutoMapper;
using Contracts.Base;
using Contracts.DataShaping;
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
using Shared.DTOs.Sample.Company;
using Shared.DTOs.Sample.Employee;

namespace Service.Base;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<IAuthenticationService> _authenticationService;


    public ServiceManager(IRepositoryManager repositoryManager,
                          ILoggerManager logger,
                          IMapper mapper,
                          UserManager<User> userManager,
                          IOptions<JwtConfiguration> configuration,
                          IDataShaper<CompanyDto> companyDataShaper,
                          IDataShaper<EmployeeDto> employeeDataShaper,
                          ICompanyLinks companyLinks,
                          IEmployeeLinks employeeLinks)
    {
        _companyService = new Lazy<ICompanyService>(() =>
                          new CompanyService(repositoryManager,
                                                      logger,
                                                      mapper,
                                             companyDataShaper,
                                             companyLinks));


        _employeeService = new Lazy<IEmployeeService>(() =>
                           new EmployeeService(repositoryManager,
                                                        logger,
                                                        mapper,
                                              employeeDataShaper,
                                                        employeeLinks));


        _authenticationService = new Lazy<IAuthenticationService>(() =>
                                 new AuthenticationService(logger,
                                                           mapper,
                                                           userManager,
                                                           configuration));
    }


    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}
