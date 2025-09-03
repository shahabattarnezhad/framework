using Contracts.Interfaces.Authentication;
using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Repository.Extensions.Authentication;
using Shared.RequestFeatures.Authentication;
using Shared.RequestFeatures.Base;

namespace Repository.Repositories.Authentication;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }



    public async Task<PagedList<User>> GetAllAsync
        (UserParameters entityParameters, bool trackChanges, CancellationToken cancellationToken)
    {
        var entities = await FindAll(trackChanges).
                                Include(entity => entity.ProfileImage).
                                Search(entityParameters.SearchTerm!).
                                Sort(entityParameters.OrderBy!).
                                Skip((entityParameters.PageNumber - 1) * entityParameters.PageSize).
                                Take(entityParameters.PageSize).
                                ToListAsync(cancellationToken);

        var count = await FindAll(trackChanges).CountAsync(cancellationToken);

        return new PagedList<User>(entities, count,
                                             entityParameters.PageNumber,
                                             entityParameters.PageSize);
    }

    public async Task<PagedList<User>> GetAllWithRolesAsync
        (UserParameters entityParameters, bool trackChanges, CancellationToken cancellationToken)
    {
        var query = FindAll(trackChanges)
        .Include(entity => entity.ProfileImage!)
        .Include(entity => entity.UserRoles!)
        .ThenInclude(ur => ur.Role)
        .Search(entityParameters.SearchTerm!)
        .Sort(entityParameters.OrderBy!);

        var count = await query.CountAsync(cancellationToken);

        var entities = await query
            .Skip((entityParameters.PageNumber - 1) * entityParameters.PageSize)
            .Take(entityParameters.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<User>(entities, count,
            entityParameters.PageNumber,
            entityParameters.PageSize);
    }


    public async Task<IEnumerable<User>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken) =>
                   await FindAll(trackChanges).
                   OrderBy(c => c.UserName).
                   ToListAsync(cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, bool trackChanges, CancellationToken cancellationToken = default) =>
                   await FindByCondition(entity =>
                   entity.Email.Equals(email), trackChanges).
                   SingleOrDefaultAsync(cancellationToken);

    public async Task<User?> GetByIdAsync(string id, bool trackChanges, CancellationToken cancellationToken = default) =>
                   await FindByCondition(entity =>
                   entity.Id.Equals(id), trackChanges)
                   .Include(entity => entity.ProfileImage).
                   SingleOrDefaultAsync(cancellationToken);

    public async Task<User?> GetByUserNameAsync(string userName, bool trackChanges, CancellationToken cancellationToken = default) =>
                   await FindByCondition(entity =>
                   entity.UserName.Equals(userName), trackChanges).
                   SingleOrDefaultAsync(cancellationToken);

    public async Task<bool> CheckIfUserExistsAsync(string userId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var user = await FindByCondition(u => u.Id.Equals(userId), trackChanges)
                              .SingleOrDefaultAsync(cancellationToken);
        if (user is null)
            return false;

        return true;
    }

    public async Task<bool> ExistsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindAll(trackChanges).AnyAsync(cancellationToken);
    }
}
