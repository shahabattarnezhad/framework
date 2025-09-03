using Microsoft.AspNetCore.Identity;

namespace Entities.Models.Authentication;

public class User : IdentityUser
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? NationalCode { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<UserRole>? UserRoles { get; set; }
}
