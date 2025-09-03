namespace Shared.DTOs.Authentication;

public record UserForPatchDto
{
    public bool IsActive { get; init; }
}
