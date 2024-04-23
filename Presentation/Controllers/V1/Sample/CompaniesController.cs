using Microsoft.AspNetCore.Mvc;
using Presentation.Utilities.ModelBinders;
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

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdCompany =
            _service.CompanyService.Create(company);

        return CreatedAtRoute("CompanyById", new
        {
            id = createdCompany.Id
        }, createdCompany);
    }


    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public IActionResult GetCompanyCollection
        ([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var entities =
            _service.CompanyService.GetByIds(ids, trackChanges: false);

        return Ok(entities);
    }


    [HttpPost("collection")]
    public IActionResult CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyForCreationDto> entityCollection)
    {
        var result =
            _service.CompanyService.CreateEntityCollection(entityCollection);

        return CreatedAtRoute("CompanyCollection", new
        {
            result.ids
        },
        result.entities);
    }


    [HttpDelete("{id:guid}")]
    public IActionResult DeleteCompany(Guid id)
    {
        _service.CompanyService.Delete(id, trackChanges: false);

        return NoContent();
    }


    [HttpPut("{id:guid}")]
    public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
    {
        if (company is null)
            return BadRequest("CompanyForUpdateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        _service.CompanyService.Update(id,
                                       company,
                                                    trackChanges: true);

        return NoContent();
    }
}
