namespace Shared.DTOs.Authentication;

public record UserForDisplayDto
{
    public string? Id { get; init; }

    public string? UserName { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string? Email { get; init; }

    public bool IsActive { get; init; }

    public List<string>? Roles { get; set; }

    public string? ProfileImageSmallUrl { get; init; }
}
