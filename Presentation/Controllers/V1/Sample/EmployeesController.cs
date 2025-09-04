using Entities.LinkModels.Sample;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Presentation.Extensions;
using Presentation.Utilities.ActionFilters;
using Service.Contracts.Base;
using Shared.DTOs.Sample.Employee;
using Shared.RequestFeatures.Sample;
using System.Text.Json;
using System.Threading;

namespace Presentation.Controllers.V1.Sample;


[Route("api/companies/{companyId}/employees")]
[ApiController]
//[OutputCache(PolicyName = "120SecondsDuration")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private const string EntityName = "Employee";

    private readonly IServiceManager _service;
    public EmployeesController(IServiceManager service) => _service = service;



    [HttpOptions]
    public IActionResult GetEmployeesOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

        return Ok();
    }


    [HttpGet]
    [HttpHead]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    [OutputCache(PolicyName = "EmployeeListCachePolicy")]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,
        [FromQuery] EmployeeParameters employeeParameters)
    {
        var linkParams = new EmployeeLinkParameters
            (employeeParameters, HttpContext);

        var result = await _service.EmployeeService
            .GetAllAsync(companyId, linkParams, trackChanges: false);

        Response.Headers
            .Add("X-Pagination",
            JsonSerializer.Serialize(result.metaData));

        return result.linkResponse.HasLinks ?
            Ok(result.linkResponse.LinkedEntities) :
            Ok(result.linkResponse.ShapedEntities);
    }


    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var result = await _service
            .EmployeeService.GetAsync(companyId, id, trackChanges: false);

        return Ok(result);
    }


    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId,
                                       [FromBody] EmployeeForCreationDto employee, CancellationToken cancellationToken)
    {
        var employeeToReturn = await _service
                                               .EmployeeService
                                               .CreateEmployeeForCompanyAsync(companyId,
                                               employee,
                                               trackChanges: false);

        // Invalidate list
        await this.InvalidateEntityCacheAsync(EntityName, cancellationToken);

        // Invalidate list + detail
        //await this.InvalidateEntityCacheAsync(EntityName, employeeToReturn.Id, cancellationToken);

        // Invalidate list + detail + several entities at same time
        //await this.InvalidateEntitiesCacheAsync(new[] { "User", "Company", "Employee" }, cancellationToken);

        return CreatedAtRoute("GetEmployeeForCompany",
            new
            {
                companyId,
                id = employeeToReturn.Id
            },
               employeeToReturn);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, CancellationToken cancellationToken)
    {
        await _service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId,
                                                                     id,
                                                                     trackChanges: false);

        // Invalidate list
        await this.InvalidateEntityCacheAsync(EntityName, cancellationToken);

        return NoContent();
    }


    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompanyAsync(Guid companyId,
                                                  Guid id,
                                                  [FromBody] EmployeeForUpdateDto employee, CancellationToken cancellationToken)
    {
        await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId,
                                                          id,
                                                          employee,
                                                          companyTrackChanges: false,
                                                          employeeTrackChanges: true);

        // Invalidate list
        await this.InvalidateEntityCacheAsync(EntityName, cancellationToken);

        return NoContent();
    }


    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompanyAsync(Guid companyId,
                                                           Guid id,
                [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc, CancellationToken cancellationToken)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object sent from client is null.");

        var result =
            await _service.EmployeeService.GetEmployeeForPatchAsync(companyId,
                                                         id,
                                                         companyTrackChanges: false,
                                                         employeeTrackChanges: true);

        patchDoc.ApplyTo(result.entityToPatch, ModelState);

        TryValidateModel(result.entityToPatch);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.EmployeeService.SaveChangesForPatchAsync(result.entityToPatch,
                                                          result.entity);

        // Invalidate list
        await this.InvalidateEntityCacheAsync(EntityName, cancellationToken);

        return NoContent();
    }
}
