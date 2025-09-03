using Entities.Models.Authentication;

namespace Contracts.Interfaces.Authentication;

public interface IUserProfileImageRepository
{
    Task<IEnumerable<UserProfileImage>> GetAllByUserIdAsync
        (string userId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<UserProfileImage> GetByUserIdAsync
        (string userId, Guid imageId, bool trackChanges, CancellationToken cancellationToken = default);

    void DeleteEntity(UserProfileImage userProfileImage);

    void CreateEntity(UserProfileImage userProfileImage);

    void CreateUserProfileImageForUser(string userId, UserProfileImage userProfileImage);

}
