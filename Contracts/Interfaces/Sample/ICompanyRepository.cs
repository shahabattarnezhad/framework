using Entities.Models.Sample;
using Shared.RequestFeatures.Base;
using Shared.RequestFeatures.Sample;

namespace Contracts.Interfaces.Sample;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync(bool trackChanges);

    Task<PagedList<Company>> GetAllAsync(CompanyParameters entityParameters, 
                                         bool trackChanges);

    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

    Task<Company>? GetDuplicateNameAsync(string entityName, bool trackChanges);

    Task<Company>? GetAsync(Guid entityId, bool trackChanges);

    void CreateEntity(Company company);

    void DeleteEntity(Company company);
}
