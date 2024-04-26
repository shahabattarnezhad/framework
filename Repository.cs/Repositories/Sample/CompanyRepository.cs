using Contracts.Interfaces.Sample;
using Entities.Models.Sample;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Repository.Extensions.Sample;
using Shared.RequestFeatures.Base;
using Shared.RequestFeatures.Sample;
using System.ComponentModel.Design;

namespace Repository.Repositories.Sample;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }



    public async Task<PagedList<Company>> GetAllAsync(CompanyParameters entityParameters,
                                                bool trackChanges)
    {
        var entities = await FindAll(trackChanges).
                                Search(entityParameters.SearchTerm!).
                                Sort(entityParameters.OrderBy!).
                                Skip((entityParameters.PageNumber - 1) *
                                            entityParameters.PageSize).
                                Take(entityParameters.PageSize).
                                ToListAsync();

        var count = await FindAll(trackChanges)
                             .CountAsync();

        return new PagedList<Company>(entities,
                                             count,
                                             entityParameters.PageNumber,
                                             entityParameters.PageSize);
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
