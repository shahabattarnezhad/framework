using Entities.Models.LinkModels.Base;
using Shared.DTOs.Sample.Employee;
using Microsoft.AspNetCore.Http;

namespace Contracts.Links.Sample;

public interface IEmployeeLinks
{
    LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto,
                                  string fields,
                                  Guid companyId,
                                  HttpContext httpContext);
}
