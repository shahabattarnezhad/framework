using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Utilities.ActionFilters;
using Service.Contracts.Base;
using Shared.DTOs.Authentication;

namespace Presentation.Controllers.V1.Authentication;


[Route("api/authentication")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _service;
    public AuthenticationController(IServiceManager service) => _service = service;


    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
    {
        var result =
            await _service.AuthenticationService.RegisterUser(userForRegistration);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }


    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
    {
        if (!await _service.AuthenticationService.ValidateUser(user))
            return Unauthorized();

        var tokenDto =
            await _service.AuthenticationService.CreateToken(populateExp: true);

        _service.AuthenticationService.SetTokensInsideCookie(tokenDto, HttpContext);

        return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    //[Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userName = User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(userName))
            return Unauthorized();

        var user = await _service.AuthenticationService.GetCurrentUserAsync(userName);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        //Response.Cookies.Delete("accessToken");
        //Response.Cookies.Delete("refreshToken");

        Response.Cookies.Delete("accessToken", new CookieOptions
        {
            HttpOnly = true,
            //Secure = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            //Domain = "example.com" // دامنه‌ی واقعی پروژه
        });
        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            //Secure = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            //Domain = "example.com"
        });

        return Ok(new { Message = "Logout successful." });
    }

    [Authorize]
    [HttpGet("verify")]
    public IActionResult Verify()
    {
        return Ok(new { Message = "Authenticated" });
    }
}
