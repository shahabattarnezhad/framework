using Contracts.Interfaces.Sample;
using Entities.Models.Sample;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;

namespace Repository.Repositories.Sample;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

    

    public async Task<IEnumerable<Company>> GetAllAsync(bool trackChanges) =>
                                            await FindAll(trackChanges).
                                            OrderBy(c => c.Name).
                                            ToListAsync();


    public async Task<Company>? GetDuplicateNameAsync(string entityName, bool trackChanges) =>
                    await FindByCondition(entity =>
                    entity.Name.Equals(entityName), trackChanges).
                    SingleOrDefaultAsync();


    public async Task<Company>? GetAsync(Guid entityId, bool trackChanges) =>
                   await FindByCondition(entity => 
                   entity.Id.Equals(entityId), trackChanges).
                   SingleOrDefaultAsync();


    public void CreateEntity(Company company) => Create(company);


    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
        await FindByCondition(x => 
                                 ids.Contains(x.Id), trackChanges)
                                    .ToListAsync();


    public void DeleteEntity(Company company) => Delete(company);
}
