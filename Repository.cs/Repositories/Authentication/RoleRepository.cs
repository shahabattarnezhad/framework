using Contracts.Interfaces.Authentication;
using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Repository.Extensions.Authentication;
using Shared.RequestFeatures.Authentication;
using Shared.RequestFeatures.Base;

namespace Repository.Repositories.Authentication;

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    public RoleRepository(RepositoryContext repositoryContext): base(repositoryContext)
    {
    }



    public async Task<PagedList<Role>> GetAllAsync
        (RoleParameters entityParameters, bool trackChanges, CancellationToken cancellationToken)
    {
        var entities = await FindAll(trackChanges).
                                Search(entityParameters.SearchTerm!).
                                Sort(entityParameters.OrderBy!).
                                Skip((entityParameters.PageNumber - 1) * entityParameters.PageSize).
                                Take(entityParameters.PageSize).
                                ToListAsync(cancellationToken);

        var count = await FindAll(trackChanges).CountAsync(cancellationToken);

        return new PagedList<Role>(entities, count,
                                             entityParameters.PageNumber,
                                             entityParameters.PageSize);
    }


    public async Task<IEnumerable<Role>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken) =>
                   await FindAll(trackChanges).
                   OrderBy(c => c.Name).
                   ToListAsync(cancellationToken);

    public async Task<Role?> GetByIdAsync(string id, bool trackChanges, CancellationToken cancellationToken = default) =>
                   await FindByCondition(entity =>
                   entity.Id.Equals(id), trackChanges).
                   SingleOrDefaultAsync(cancellationToken);

    public async Task<Role?> GetByRoleNameAsync(string roleName, bool trackChanges, CancellationToken cancellationToken = default) =>
                   await FindByCondition(entity =>
                   entity.Name.Equals(roleName), trackChanges).
                   SingleOrDefaultAsync(cancellationToken);
}
