using Microsoft.AspNetCore.Identity;

namespace Entities.Models.Authentication;

public class UserRole : IdentityUserRole<string>
{
    public Role? Role { get; set; }

    public User? User { get; set; }
}
