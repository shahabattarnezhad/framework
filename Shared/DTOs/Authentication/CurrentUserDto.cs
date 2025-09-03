namespace Shared.DTOs.Authentication;

public record CurrentUserDto
{
    public string? UserName { get; init; }

    public string? Email { get; init; }

    public string[]? Roles { get; init; }
}
