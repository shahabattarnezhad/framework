using Shared.DTOs.Sample.Company;

namespace Service.Contracts.Interfaces;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAll(bool trackChanges);

    CompanyDto Get(Guid entityId, bool trackChanges);
}
