using Microsoft.AspNetCore.Identity;

namespace Shared.DTOs.Authentication;

public class UserValidationResult
{
    public bool IsValid => !Errors.Any();

    public List<IdentityError> Errors { get; set; } = new();
}
