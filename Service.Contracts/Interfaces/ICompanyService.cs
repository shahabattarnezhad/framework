using Shared.DTOs.Sample.Company;

namespace Service.Contracts.Interfaces;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAll(bool trackChanges);

    IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

    CompanyDto Get(Guid entityId, bool trackChanges);

    CompanyDto Create(CompanyForCreationDto company);

    (IEnumerable<CompanyDto> entities, string ids) CreateEntityCollection
        (IEnumerable<CompanyForCreationDto> entityCollection);

    void Delete(Guid companyId, bool trackChanges);
}
