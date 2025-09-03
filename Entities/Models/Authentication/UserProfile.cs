using Entities.Models.Base;

namespace Entities.Models.Authentication;

public class UserProfile : BaseEntity<Guid>
{
    public string? Bio { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }
}
