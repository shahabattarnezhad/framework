namespace Shared.DTOs.Authentication;

public record UserDto
{
    public string? Id { get; init; }

    public string? UserName { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string? NationalCode { get; init; }

    public string FullName => $"{FirstName} {LastName}";

    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }

    public bool IsActive { get; init; }

    public List<string>? Roles { get; set; }

    public string? ProfileImageSmallUrl { get; init; }

    public string? ProfileImageMediumUrl { get; init; }

    public string? ProfileImageLargeUrl { get; init; }

    public string? ProfileImageOriginalUrl { get; init; }
}
