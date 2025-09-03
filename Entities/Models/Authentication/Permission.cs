using Entities.Models.Base;

namespace Entities.Models.Authentication;

public class Permission : BaseEntity<Guid>
{
    public string? Name { get; set; }

    public string? DisplayName { get; set; }

    public ICollection<RolePermission>? RolePermissions { get; set; }
}
