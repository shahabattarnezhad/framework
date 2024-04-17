using Contracts.Interfaces.Sample;
using Entities.Models.Sample;
using Repository.Base;
using Repository.Data;

namespace Repository.Repositories.Sample;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

    

    public IEnumerable<Company> GetAll(bool trackChanges) =>
                                FindAll(trackChanges).
                                OrderBy(c => c.Name).
                                ToList();


    public Company? Get(Guid entityId, bool trackChanges) =>
                   FindByCondition(entity => 
                   entity.Id.Equals(entityId), trackChanges).
                   SingleOrDefault();


    public void CreateEntity(Company company) => Create(company);
}
