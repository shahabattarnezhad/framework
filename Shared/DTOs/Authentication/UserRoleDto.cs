namespace Shared.DTOs.Authentication;

public record UserRoleDto
{
    public string? RoleId { get; init; }
    public string? RoleName { get; init; }
}
