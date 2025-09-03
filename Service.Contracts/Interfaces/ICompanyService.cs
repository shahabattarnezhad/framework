using Entities.LinkModels.Base;
using Entities.LinkModels.Sample;
using Shared.DTOs.Sample.Company;
using Shared.RequestFeatures.Base;

namespace Service.Contracts.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllAsync(bool trackChanges);

    Task<(LinkResponse linkResponse, MetaData metaData)> GetAllAsync
        (CompanyLinkParameters linkParameters, bool trackChanges);

    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

    Task<CompanyDto>? GetDuplicateNameAsync(string entityName, bool trackChanges);

    Task<CompanyDto>? GetAsync(Guid entityId, bool trackChanges);

    Task<CompanyDto> CreateAsync(CompanyForCreationDto entityForCreation);

    Task<(IEnumerable<CompanyDto> entities, string ids)> CreateEntityCollectionAsync
        (IEnumerable<CompanyForCreationDto> entityCollection);

    Task DeleteAsync(Guid entityId, bool trackChanges);

    Task UpdateAsync(Guid entityId, CompanyForUpdateDto entityForUpdate, bool trackChanges);
}
