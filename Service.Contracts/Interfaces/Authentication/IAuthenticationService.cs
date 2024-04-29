using Microsoft.AspNetCore.Identity;
using Shared.DTOs.Authentication;

namespace Service.Contracts.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);

    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    
    Task<string> CreateToken();
}
