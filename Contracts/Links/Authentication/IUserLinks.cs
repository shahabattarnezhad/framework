using Entities.LinkModels.Base;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Authentication;

namespace Contracts.Links.Authentication;

public interface IUserLinks
{
    //LinkResponse TryGenerateLinks(IEnumerable<UserDto> usersDto, string fields, HttpContext httpContext);
    LinkResponse TryGenerateLinks(IEnumerable<UserForDisplayDto> usersDto, string fields, HttpContext httpContext);
}
