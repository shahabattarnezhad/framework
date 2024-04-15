using Shared.DTOs.Sample.Company;

namespace Service.Contracts.Interfaces;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAll(bool trackChanges);
}
