using Entities.Models.LinkModels.Base;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Sample.Company;

namespace Contracts.Links.Sample;

public interface ICompanyLinks
{
    LinkResponse TryGenerateLinks(IEnumerable<CompanyDto> companiesDto,
                                  string fields,
                                  HttpContext httpContext);
}
