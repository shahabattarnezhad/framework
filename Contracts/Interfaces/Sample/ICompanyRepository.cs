using Entities.Models.Sample;

namespace Contracts.Interfaces.Sample;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAll(bool trackChanges);
}
