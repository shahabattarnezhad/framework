﻿using Entities.Models.Base;
using Shared.DTOs.Sample.Company;
using Shared.RequestFeatures.Base;
using Shared.RequestFeatures.Sample;

namespace Service.Contracts.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllAsync(bool trackChanges);

    Task<(IEnumerable<Entity> entities, MetaData metaData)> GetAllAsync
        (CompanyParameters entityParameters, bool trackChanges);

    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

    Task<CompanyDto>? GetDuplicateNameAsync(string entityName, bool trackChanges);

    Task<CompanyDto>? GetAsync(Guid entityId, bool trackChanges);

    Task<CompanyDto> CreateAsync(CompanyForCreationDto entityForCreation);

    Task<(IEnumerable<CompanyDto> entities, string ids)> CreateEntityCollectionAsync
        (IEnumerable<CompanyForCreationDto> entityCollection);

    Task DeleteAsync(Guid entityId, bool trackChanges);

    Task UpdateAsync(Guid entityId, CompanyForUpdateDto entityForUpdate, bool trackChanges);
}
