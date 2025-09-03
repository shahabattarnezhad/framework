using Entities.Models.Authentication;
using Microsoft.AspNetCore.Http;

namespace Service.Contracts.Interfaces.Authentication;

public interface IUserProfileImageService
{
    Task<UserProfileImage> SaveUserProfileImageAsync
        (string userId, IFormFile file, string imageType, CancellationToken cancellationToken);

    Task DeleteUserProfileImageAsync(string userId, Guid imageId, CancellationToken cancellationToken);
}
