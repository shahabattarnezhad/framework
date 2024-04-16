using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Base;

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


    [HttpGet("{id:guid}")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var result =
            _service.EmployeeService.Get(companyId, id, trackChanges: false);

        return Ok(result);
     }
}
