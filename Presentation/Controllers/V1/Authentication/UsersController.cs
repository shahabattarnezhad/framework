using Entities.Exceptions.General;
using Entities.LinkModels.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.Utilities.ActionFilters;
using Service.Contracts.Base;
using Shared.DTOs.Authentication;
using Shared.RequestFeatures.Authentication;
using System.Text.Json;

namespace Presentation.Controllers.V1.Authentication;


[Route("api/users")]
[ApiController]
//[OutputCache(PolicyName = "120SecondsDuration")]
[Authorize(Roles = "Admin")]
[ApiExplorerSettings(GroupName = "v1")]
//[EnableRateLimiting("SpecificPolicy")] for the whole controller
public class UsersController : ControllerBase
{
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
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetUsers([FromQuery] UserParameters userParameters, CancellationToken cancellationToken)
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
    public async Task<IActionResult> GetUserById(string id, CancellationToken cancellationToken)
    {
        var result =
            await _service.UserService.GetByIdAsync(id, false, cancellationToken)!;

        return Ok(result);
    }


    [HttpGet("by-email/{email}", Name = "UserByEmail")]
    //[OutputCache(Duration = 60)]
    //[EnableRateLimiting("SpecificPolicy")] for certain endpoint
    //[DisableRateLimiting] for disabling the rate limiting for certain endpoint
    public async Task<IActionResult> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var result =
            await _service.UserService.GetByEmailAsync(email, false, cancellationToken)!;

        return Ok(result);
    }


    [HttpGet("by-username/{username}", Name = "UserByUsername")]
    //[OutputCache(Duration = 60)]
    //[EnableRateLimiting("SpecificPolicy")] for certain endpoint
    //[DisableRateLimiting] for disabling the rate limiting for certain endpoint
    public async Task<IActionResult> GetUserByUserName(string username, CancellationToken cancellationToken)
    {
        var result =
            await _service.UserService.GetByUserNameAsync(username, false, cancellationToken)!;

        return Ok(result);
    }

    [HttpPatch("{userId}")]
    public async Task<IActionResult> PartiallyUpdateUserAsync(string userId, [FromBody] JsonPatchDocument<UserForPatchDto> patchDoc)
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

        return NoContent();
    }

    [HttpGet("{userId}/roles")]
    public async Task<IActionResult> GetRolesForUser(string userId, CancellationToken cancellationToken)
    {
        var roles = await _service.UserRoleService.GetRolesByUserIdAsync(userId, false, cancellationToken);

        return Ok(roles);
    }

    [HttpPost("{userId}/roles/{roleId}")]
    public async Task<IActionResult> AssignRoleToUser(string userId, string roleId, CancellationToken cancellationToken)
    {
        await _service.UserRoleService.AssignRoleToUserAsync(userId, roleId, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{userId}/roles/{roleId}")]
    public async Task<IActionResult> RemoveRoleFromUser(string userId, string roleId, CancellationToken cancellationToken)
    {
        await _service.UserRoleService.RemoveRoleFromUserAsync(userId, roleId, cancellationToken);

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("any-user-exists")]
    public async Task<IActionResult> AnyUserExists(CancellationToken cancellationToken)
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
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UserForUpdationDto user)
    {
        await _service.UserService.UpdateAsync(id, user, trackChanges: true);

        return NoContent();
    }
}
