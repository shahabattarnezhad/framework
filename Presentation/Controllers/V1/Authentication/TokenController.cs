using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Base;
using Shared.DTOs.Authentication;

namespace Presentation.Controllers.V1.Authentication;


[Route("api/token")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class TokenController : ControllerBase
{
    private readonly IServiceManager _service;
    public TokenController(IServiceManager service) => _service = service;


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken);
        HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

        var tokenDto = new TokenDto(accessToken, refreshToken);

        var tokenDtoToReturn =
            await _service.AuthenticationService.RefreshToken(tokenDto);

        _service.AuthenticationService.SetTokensInsideCookie(tokenDtoToReturn, HttpContext);

        return Ok();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var tokenDto = await _service.AuthenticationService.RefreshToken(HttpContext);

        _service.AuthenticationService.SetTokensInsideCookie(tokenDto, HttpContext);

        return Ok();
    }

    [HttpGet("is-authenticated")]
    public IActionResult IsAuthenticated()
    {
        var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        return Ok(isAuthenticated);
    }
}
