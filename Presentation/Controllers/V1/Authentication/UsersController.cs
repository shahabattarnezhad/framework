using Entities.Exceptions.General;
using Entities.LinkModels.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Presentation.Extensions;
using Presentation.Utilities.ActionFilters;
using Service.Contracts.Base;
using Shared.DTOs.Authentication;
using Shared.RequestFeatures.Authentication;
using System.Text.Json;
using System.Threading;

namespace Presentation.Controllers.V1.Authentication;


[Route("api/users")]
[ApiController]
//[OutputCache(PolicyName = "120SecondsDuration")]
[Authorize(Roles = "Admin")]
[ApiExplorerSettings(GroupName = "v1")]
//[EnableRateLimiting("SpecificPolicy")] for the whole controller
public class UsersController : ControllerBase
{
    private const string EntityName = "User";

    private readonly IServiceManager _service;

    public UsersController(IServiceManager service) => _service = service;



    [HttpOptions]
    public IActionResult GetUsersOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

        return Ok();
    }



    /// <summary>
    /// Gets the list of all users
    /// </summary>
    /// <param name="userParameters"></param>
    /// <returns>The users list</returns>
    [HttpGet(Name = "GetUsers")]
    [HttpHead]
    //[OutputCache(PolicyName = "120SecondsDuration", Tags = new[] { "UserCache" })]
    [OutputCache(PolicyName = "UserListCachePolicy")]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetUsersAsync([FromQuery] UserParameters userParameters, CancellationToken cancellationToken)
    {
        var linkParams = new UserLinkParameters(userParameters, HttpContext);

        var result =
            await _service.UserService.GetAllWithRolesAsync(linkParams, false, cancellationToken);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

        return result.linkResponse.HasLinks ?
            Ok(result.linkResponse.LinkedEntities) :
            Ok(result.linkResponse.ShapedEntities);
    }


    [HttpGet("by-id/{id}", Name = "UserById")]
    //[OutputCache(Duration = 60)]
    //[EnableRateLimiting("SpecificPolicy")] for certain endpoint
    //[DisableRateLimiting] for disabling the rate limiting for certain endpoint
    public async Task<IActionResult> GetUserByIdAsync(string id, CancellationToken cancellationToken)
    {
        var result =
            await _service.UserService.GetByIdAsync(id, false, cancellationToken)!;

        return Ok(result);
    }


    [HttpGet("by-email/{email}", Name = "UserByEmail")]
    //[OutputCache(Duration = 60)]
    //[EnableRateLimiting("SpecificPolicy")] for certain endpoint
    //[DisableRateLimiting] for disabling the rate limiting for certain endpoint
    public async Task<IActionResult> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var result =
            await _service.UserService.GetByEmailAsync(email, false, cancellationToken)!;

        return Ok(result);
    }


    [HttpGet("by-username/{username}", Name = "UserByUsername")]
    //[OutputCache(Duration = 60)]
    //[EnableRateLimiting("SpecificPolicy")] for certain endpoint
    //[DisableRateLimiting] for disabling the rate limiting for certain endpoint
    public async Task<IActionResult> GetUserByUserNameAsync(string username, CancellationToken cancellationToken)
    {
        var result =
            await _service.UserService.GetByUserNameAsync(username, false, cancellationToken)!;

        return Ok(result);
    }

    [HttpPatch("{userId}")]
    public async Task<IActionResult> PartiallyUpdateUserAsync
        (string userId, [FromBody] JsonPatchDocument<UserForPatchDto> patchDoc, CancellationToken cancellationToken)
    {
        var currentUser =
            _service.AuthenticationService.IsCurrentUser(userId, HttpContext);

        if (currentUser)
            throw new UserCannotChangeThemselvesException();

        if (patchDoc is null)
            return BadRequest("PatchDoc object sent from client is null.");

        var result =
            await _service.UserService.GetUserForPatchAsync(userId, true);

        patchDoc.ApplyTo(result.entityToPatch, ModelState);

        TryValidateModel(result.entityToPatch);

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v =>
            v.Errors).Select(e => e.ErrorMessage);
            return UnprocessableEntity(new { Errors = errors });
        }

        await _service.UserService.SaveChangesForPatchAsync(result.entityToPatch, result.entity);

        // Invalidate list
        await this.InvalidateEntityCacheAsync(EntityName, cancellationToken);

        return NoContent();
    }

    [HttpGet("{userId}/roles")]
    public async Task<IActionResult> GetRolesForUserAsync(string userId, CancellationToken cancellationToken)
    {
        var roles = await _service.UserRoleService.GetRolesByUserIdAsync(userId, false, cancellationToken);

        return Ok(roles);
    }

    [HttpPost("{userId}/rolename/{roleId}")]
    public async Task<IActionResult> AssignRoleToUserAsync(string userId, string roleId, CancellationToken cancellationToken)
    {
        await _service.UserRoleService.AssignRoleToUserAsync(userId, roleId, cancellationToken);

        // Invalidate list
        await this.InvalidateEntityCacheAsync(EntityName, cancellationToken);

        return NoContent();
    }



    [HttpPost("{userId}/roles/{roleName}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRoleToUserByRoleNameAsync(string userId, string roleName, CancellationToken cancellationToken)
    {
        await _service.AuthenticationService.AssignRoleToUserAsync(userId, roleName, cancellationToken);

        // Invalidate list
        await this.InvalidateEntityCacheAsync(EntityName, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{userId}/roles/{roleId}")]
    public async Task<IActionResult> RemoveRoleFromUserAsync(string userId, string roleId, CancellationToken cancellationToken)
    {
        await _service.UserRoleService.RemoveRoleFromUserAsync(userId, roleId, cancellationToken);

        // Invalidate list
        await this.InvalidateEntityCacheAsync(EntityName, cancellationToken);

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("any-user-exists")]
    public async Task<IActionResult> AnyUserExistsAsync(CancellationToken cancellationToken)
    {
        var exists = await _service.UserService.AnyUserExistsAsync(false, cancellationToken);
        return Ok(exists);
    }


    //[HttpDelete("{id:string}")]
    //public async Task<IActionResult> DeleteUser(string id)
    //{
    //    await _service.UserService.DeleteAsync(id, trackChanges: false);

    //    return NoContent();
    //}


    [HttpPut("{id}")]
    //[HttpPut("{id:string}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] UserForUpdationDto user, CancellationToken cancellationToken)
    {
        await _service.UserService.UpdateAsync(id, user, trackChanges: true);

        // Invalidate list
        await this.InvalidateEntityCacheAsync(EntityName, cancellationToken);

        return NoContent();
    }
}
