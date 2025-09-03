using Entities.LinkModels.Base;
using Entities.LinkModels.Sample;
using Entities.Models.Sample;
using Shared.DTOs.Sample.Employee;
using Shared.RequestFeatures.Base;

namespace Service.Contracts.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync(Guid companyId, bool trackChanges);

    Task<(LinkResponse linkResponse, MetaData metaData)> GetAllAsync
                                              (Guid companyId,
                                               EmployeeLinkParameters linkParameters,
                                               bool trackChanges);

    Task<EmployeeDto> GetAsync(Guid companyId, Guid id, bool trackChanges);

    Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId,
                                             EmployeeForCreationDto employeeForCreation,
                                             bool trackChanges);

    Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);

    Task UpdateEmployeeForCompanyAsync(Guid companyId,
                                       Guid id,
                                       EmployeeForUpdateDto entityForUpdate,
                                       bool companyTrackChanges,
                                       bool employeeTrackChanges);

    Task<(EmployeeForUpdateDto entityToPatch, Employee entity)> GetEmployeeForPatchAsync
                           (Guid companyId,
                            Guid id,
                            bool companyTrackChanges,
                            bool employeeTrackChanges);

    Task SaveChangesForPatchAsync(EmployeeForUpdateDto entityToPatch, Employee entity);
}
