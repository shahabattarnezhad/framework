using Entities.Models.Sample;

namespace Contracts.Interfaces.Sample;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAll(bool trackChanges);

    IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

    Company? GetDuplicateName(string entityName, bool trackChanges);

    Company? Get(Guid entityId, bool trackChanges);

    void CreateEntity(Company company);

    void DeleteEntity(Company company);
}
