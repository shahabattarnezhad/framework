using Entities.Models.Administrative;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models.Authentication;

public class Role : IdentityRole<string>
{
    public ICollection<UserRole>? UserRoles { get; set; }
    public ICollection<NoticeRole>? NoticeRoles { get; set; }
    public ICollection<RolePermission>? RolePermissions { get; set; }
}
