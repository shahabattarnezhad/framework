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
}
