using Entities.Models.Base;

namespace Entities.Models.Authentication;

public class RolePermission
{
    public string? RoleId { get; set; }
    public Role? Role { get; set; }

    public Guid PermissionId { get; set; }
    public Permission? Permission { get; set; }
}
