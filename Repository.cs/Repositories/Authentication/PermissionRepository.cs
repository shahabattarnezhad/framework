using Contracts.Interfaces.Authentication;
using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;

namespace Repository.Repositories.Authentication;

public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
{
    public PermissionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<Permission>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken) =>
                   await FindAll(trackChanges).
                   OrderBy(c => c.Name).
                   ToListAsync(cancellationToken);

    public async Task<Permission?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default) =>
                   await FindByCondition(entity =>
                   entity.Id.Equals(id), trackChanges).
                   SingleOrDefaultAsync(cancellationToken);

    public async Task<bool> ExistsAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default) =>
        await FindByCondition(entity => entity.Id.Equals(id), trackChanges)
              .AnyAsync(cancellationToken);
}
