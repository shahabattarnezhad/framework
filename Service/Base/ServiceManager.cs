using AutoMapper;
using Contracts.Base;
using Contracts.DataShaping;
using Contracts.Links.Sample;
using Contracts.Logging;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Service.Services.Sample;
using Shared.DTOs.Sample.Company;
using Shared.DTOs.Sample.Employee;

namespace Service.Base;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService; 
    private readonly Lazy<IEmployeeService> _employeeService;


    public ServiceManager(IRepositoryManager repositoryManager,
                          ILoggerManager logger,
                          IMapper mapper,
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
    }


    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}
