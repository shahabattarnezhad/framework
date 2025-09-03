using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs.Authentication;

namespace Service.Contracts.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration, CancellationToken cancellationToken = default);

    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth, CancellationToken cancellationToken = default);

    Task<TokenDto> CreateToken(bool populateExp, CancellationToken cancellationToken = default);

    Task<TokenDto> RefreshToken(TokenDto tokenDto, CancellationToken cancellationToken = default);

    Task<TokenDto> RefreshToken(HttpContext context, CancellationToken cancellationToken = default);

    Task<UserDto?> GetCurrentUserAsync(string username, CancellationToken cancellationToken = default);

    Task<UserValidationResult> ValidateUserRegistrationAsync(UserForRegistrationDto dto, CancellationToken cancellationToken);

    Task<UserDto> CreateUserAsync(UserForRegistrationDto dto, CancellationToken cancellationToken);

    bool IsCurrentUser(string userId, HttpContext context, CancellationToken cancellationToken = default);

    void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context, CancellationToken cancellationToken = default);
}
