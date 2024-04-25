using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.Utilities.ActionFilters;
using Service.Contracts.Base;
using Shared.DTOs.Sample.Employee;
using Shared.RequestFeatures.Sample;

namespace Presentation.Controllers.V1.Sample;


[Route("api/companies/{companyId}/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;
    public EmployeesController(IServiceManager service) => _service = service;


    [HttpGet]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,
        [FromQuery] EmployeeParameters employeeParameters)
    {
        var results = await _service.EmployeeService
            .GetAllAsync(companyId, employeeParameters, trackChanges: false);

        return Ok(results);
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
                                       [FromBody] EmployeeForCreationDto employee)
    {
        var employeeToReturn = await _service
                                               .EmployeeService
                                               .CreateEmployeeForCompanyAsync(companyId,
                                               employee,
                                               trackChanges: false);

        return CreatedAtRoute("GetEmployeeForCompany",
            new
            {
                companyId,
                id = employeeToReturn.Id
            },
               employeeToReturn);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompanyAsync(Guid companyId, Guid id)
    {
        await _service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, 
                                                                     id,
                                                                     trackChanges: false);

        return NoContent();
    }


    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompanyAsync(Guid companyId,
                                                  Guid id,
                                                  [FromBody] EmployeeForUpdateDto employee)
    {
        await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId,
                                                          id,
                                                          employee,
                                                          companyTrackChanges: false,
                                                          employeeTrackChanges: true);
        return NoContent();
    }


    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompanyAsync(Guid companyId,
                                                           Guid id,
                [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
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

        return NoContent();
    }
}
