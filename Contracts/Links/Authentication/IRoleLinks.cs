using Entities.LinkModels.Base;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Authentication;

namespace Contracts.Links.Authentication;

public interface IRoleLinks
{
    LinkResponse TryGenerateLinks(IEnumerable<RoleDto> rolesDto, string fields, HttpContext httpContext);
}
