using Entities.LinkModels.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Utilities.ActionFilters;
using Service.Contracts.Base;
using Shared.RequestFeatures.Authentication;
using System.Text.Json;

namespace Presentation.Controllers.V1.Authentication;


[Route("api/roles")]
[ApiController]
//[OutputCache(PolicyName = "120SecondsDuration")]
[Authorize(Roles = "Admin")]
[ApiExplorerSettings(GroupName = "v1")]
//[EnableRateLimiting("SpecificPolicy")] for the whole controller
public class RolesController : ControllerBase
{
    private readonly IServiceManager _service;
    public RolesController(IServiceManager service) => _service = service;



    [HttpOptions]
    public IActionResult GetRolesOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

        return Ok();
    }



    /// <summary>
    /// Gets the list of all roles
    /// </summary>
    /// <param name="roleParameters"></param>
    /// <returns>The roles list</returns>
    [HttpGet(Name = "GetRoles")]
    [HttpHead]
    //[OutputCache(PolicyName = "120SecondsDuration", Tags = new[] { "UserCache" })]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetRoles([FromQuery] RoleParameters roleParameters, CancellationToken cancellationToken)
    {
        var linkParams = new RoleLinkParameters(roleParameters, HttpContext);

        var result =
            await _service.RoleService.GetAllAsync(linkParams, false, cancellationToken);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

        return result.linkResponse.HasLinks ?
            Ok(result.linkResponse.LinkedEntities) :
            Ok(result.linkResponse.ShapedEntities);
    }


    [HttpGet("by-id/{id}", Name = "RoleById")]
    //[OutputCache(Duration = 60)]
    //[EnableRateLimiting("SpecificPolicy")] for certain endpoint
    //[DisableRateLimiting] for disabling the rate limiting for certain endpoint
    public async Task<IActionResult> GetRoleById(string id, CancellationToken cancellationToken)
    {
        var result =
            await _service.RoleService.GetByIdAsync(id, false, cancellationToken)!;

        return Ok(result);
    }


    [HttpGet("by-rolename/{rolename}", Name = "RoleByName")]
    //[OutputCache(Duration = 60)]
    //[EnableRateLimiting("SpecificPolicy")] for certain endpoint
    //[DisableRateLimiting] for disabling the rate limiting for certain endpoint
    public async Task<IActionResult> GetRoleByRoleName(string rolename, CancellationToken cancellationToken)
    {
        var result =
            await _service.RoleService.GetByRoleNameAsync(rolename, false, cancellationToken)!;

        return Ok(result);
    }

    [HttpGet("{roleId}/permissions")]
    public async Task<IActionResult> GetPermissionsForRole(string roleId, CancellationToken cancellationToken)
    {
        var permissions = await _service.RolePermissionService
                                        .GetPermissionsByRoleIdAsync(roleId, false, cancellationToken);

        return Ok(permissions);
    }

    [HttpPost("{roleId}/permissions/{permissionId}")]
    public async Task<IActionResult> AssignPermissionToRole(string roleId, Guid permissionId, CancellationToken cancellationToken)
    {
        await _service.RolePermissionService
                      .AssignPermissionToRoleAsync(roleId, permissionId, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{roleId}/permissions/{permissionId}")]
    public async Task<IActionResult> RemovePermissionFromRole(string roleId, Guid permissionId, CancellationToken cancellationToken)
    {
        await _service.RolePermissionService
                      .RemovePermissionFromRoleAsync(roleId, permissionId, cancellationToken);

        return NoContent();
    }
}
