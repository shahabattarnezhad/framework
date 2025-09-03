using Shared.DTOs.Base;

namespace Shared.DTOs.Authentication;

public record UserProfileDto : BaseEntityDto<Guid>
{
    public string? Bio { get; init; }

    public string? ProfilePictureUrl { get; init; }

    public DateTime? BirthDate { get; init; }

    public string? UserId { get; init; }
    public UserDto? UserDto { get; init; }
}
