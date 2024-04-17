using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Base;
using Shared.DTOs.Sample.Company;

namespace Presentation.Controllers.V1.Sample;


[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;
    public CompaniesController(IServiceManager service) => _service = service;


    [HttpGet]
    public IActionResult GetCompanies()
    {
        var results =
                _service.CompanyService.GetAll(trackChanges: false);

        return Ok(results);
    }


    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        var result =
            _service.CompanyService.Get(id, trackChanges: false);

        return Ok(result);
    }


    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
    {
        if (company is null)
            return BadRequest("CompanyForCreationDto object is null");

        var createdCompany =
            _service.CompanyService.Create(company);

        return CreatedAtRoute("CompanyById", new
        {
            id = createdCompany.Id
        }, createdCompany);
    }
}
