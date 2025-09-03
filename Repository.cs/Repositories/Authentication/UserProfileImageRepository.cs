using Contracts.Interfaces.Authentication;
using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;

namespace Repository.Repositories.Authentication;

public class UserProfileImageRepository : RepositoryBase<UserProfileImage>, IUserProfileImageRepository
{
    public UserProfileImageRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<UserProfileImage>> GetAllByUserIdAsync
        (string userId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.UserId.Equals(userId), trackChanges)
                     .ToListAsync(cancellationToken);
    }

    public async Task<UserProfileImage> GetByUserIdAsync
        (string userId, Guid imageId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.UserId.Equals(userId) &&
                                              entity.Id.Equals(imageId), trackChanges)
                     .SingleOrDefaultAsync(cancellationToken);
    }

    public void CreateUserProfileImageForUser(string userId, UserProfileImage userProfileImage)
    {
        userProfileImage.UserId = userId;
        Create(userProfileImage);
    }

    public void DeleteEntity(UserProfileImage userProfileImage) => Delete(userProfileImage);

    public void CreateEntity(UserProfileImage userProfileImage) => Create(userProfileImage);
}
