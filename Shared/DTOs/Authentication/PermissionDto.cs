using Shared.DTOs.Base;

namespace Shared.DTOs.Authentication;

public record PermissionDto : BaseEntityDto<Guid>
{
    public string? Name { get; init; }

    public string? DisplayName { get; init; }
}
