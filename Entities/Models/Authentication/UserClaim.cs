using Microsoft.AspNetCore.Identity;

namespace Entities.Models.Authentication;

public class UserClaim : IdentityUserClaim<string>
{
    public string? IssuedBy { get; set; }

    public DateTime CreatedAt { get; set; }
}
