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
    public async Task<IActionResult> GetCompanies()
    {
        var results =
                await _service.CompanyService.GetAllAsync(trackChanges: false);

        return Ok(results);
    }


    [HttpGet("{id:guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var result =
            await _service.CompanyService.GetAsync(id, trackChanges: false)!;

        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
    {
        if (company is null)
            return BadRequest("CompanyForCreationDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdCompany =
            await _service.CompanyService.CreateAsync(company);

        return CreatedAtRoute("CompanyById", new
        {
            id = createdCompany.Id
        }, createdCompany);
    }


    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection
        ([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var entities =
            await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);

        return Ok(entities);
    }


    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyForCreationDto> entityCollection)
    {
        var result =
            await _service.CompanyService.CreateEntityCollectionAsync(entityCollection);

        return CreatedAtRoute("CompanyCollection", new
        {
            result.ids
        },
        result.entities);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _service.CompanyService.DeleteAsync(id, trackChanges: false);

        return NoContent();
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCompanyAsync(Guid id, [FromBody] CompanyForUpdateDto company)
    {
        if (company is null)
            return BadRequest("CompanyForUpdateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.CompanyService.UpdateAsync(id,
                                        company,
                                                    trackChanges: true);

        return NoContent();
    }
}
