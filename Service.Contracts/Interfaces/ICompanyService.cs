using Shared.DTOs.Sample.Company;

namespace Service.Contracts.Interfaces;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAll(bool trackChanges);

    IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

    CompanyDto Get(Guid entityId, bool trackChanges);

    CompanyDto Create(CompanyForCreationDto entityForCreation);

    (IEnumerable<CompanyDto> entities, string ids) CreateEntityCollection
        (IEnumerable<CompanyForCreationDto> entityCollection);

    void Delete(Guid entityId, bool trackChanges);

    void Update(Guid entityId, CompanyForUpdateDto entityForUpdate, bool trackChanges);
}
