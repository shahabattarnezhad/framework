using Shared.Messages.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Authentication;

public record UserForAuthenticationDto
{
    [Required(ErrorMessage = AuthValidationMessage.UsernameValidation)]
    public string? UserName { get; init; }


    [Required(ErrorMessage = AuthValidationMessage.PasswordValidation)]
    public string? Password { get; init; }
}
