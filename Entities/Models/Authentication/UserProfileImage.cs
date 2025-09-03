namespace Entities.Models.Authentication;

public class UserProfileImage
{
    public Guid Id { get; set; }

    public string? ProfileImageSmallUrl { get; set; }

    public string? ProfileImageMediumUrl { get; set; }

    public string? ProfileImageLargeUrl { get; set; }

    public string? ProfileImageOriginalUrl { get; set; }

    public string? Type { get; set; }

    public string? AltText { get; set; }


    public string? UserId { get; set; }
    public User? User { get; set; }
}
