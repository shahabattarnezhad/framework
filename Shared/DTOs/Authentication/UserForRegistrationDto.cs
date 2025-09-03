using Shared.Messages.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Authentication;

public record UserForRegistrationDto
{
    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    [Required(ErrorMessage = AuthValidationMessage.UsernameValidation)]
    public string? UserName { get; init; }

    [Required(ErrorMessage = AuthValidationMessage.PasswordValidation)]
    public string? Password { get; init; }

    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }

    public ICollection<string>? Roles { get; init; }
}
