using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Base;
using Shared.DTOs.Sample.Employee;

namespace Presentation.Controllers.V1.Sample;


[Route("api/companies/{companyId}/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;
    public EmployeesController(IServiceManager service) => _service = service;


    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        var results =
                _service.EmployeeService.GetAll(companyId, trackChanges: false);

        return Ok(results);
    }


    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var result =
            _service.EmployeeService.Get(companyId, id, trackChanges: false);

        return Ok(result);
    }


    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId,
                                       [FromBody] EmployeeForCreationDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeForCreationDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var employeeToReturn =
            _service.EmployeeService.CreateEmployeeForCompany(companyId,
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
    public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        _service.EmployeeService
            .DeleteEmployeeForCompany(companyId, id, trackChanges: false);

        return NoContent();
    }


    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId,
                                                  Guid id,
                                                  [FromBody] EmployeeForUpdateDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeForUpdateDto object is null");

        if (!ModelState.IsValid) 
            return UnprocessableEntity(ModelState);

        _service.EmployeeService.UpdateEmployeeForCompany(companyId,
                                                          id,
                                                          employee,
                                                          companyTrackChanges: false,
                                                          employeeTrackChanges: true);
        return NoContent();
    }


    [HttpPatch("{id:guid}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId,
                                                           Guid id,
                [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object sent from client is null.");

        var result =
            _service.EmployeeService.GetEmployeeForPatch(companyId,
                                                         id,
                                                         companyTrackChanges: false,
                                                         employeeTrackChanges: true);

        patchDoc.ApplyTo(result.entityToPatch, ModelState);

        TryValidateModel(result.entityToPatch);

        if (!ModelState.IsValid) 
            return UnprocessableEntity(ModelState);

        _service.EmployeeService.SaveChangesForPatch(result.entityToPatch,
                                                          result.entity);

        return NoContent();
    }
}
