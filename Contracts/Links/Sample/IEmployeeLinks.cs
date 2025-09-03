using Shared.DTOs.Sample.Employee;
using Microsoft.AspNetCore.Http;
using Entities.LinkModels.Base;

namespace Contracts.Links.Sample;

public interface IEmployeeLinks
{
    LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto,
                                  string fields,
                                  Guid companyId,
                                  HttpContext httpContext);
}
