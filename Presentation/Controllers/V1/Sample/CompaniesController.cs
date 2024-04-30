using Entities.Models.LinkModels.Sample;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Presentation.Utilities.ActionFilters;
using Presentation.Utilities.ModelBinders;
using Service.Contracts.Base;
using Shared.DTOs.Sample.Company;
using Shared.RequestFeatures.Sample;
using System.Text.Json;

namespace Presentation.Controllers.V1.Sample;


[Route("api/companies")]
[ApiController]
[OutputCache(PolicyName = "120SecondsDuration")]
[Authorize(Roles = "Manager")]
[ApiExplorerSettings(GroupName = "v1")]
//[EnableRateLimiting("SpecificPolicy")] for the whole controller
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;
    public CompaniesController(IServiceManager service) => _service = service;



    [HttpOptions] 
    public IActionResult GetCompaniesOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

        return Ok();
    }



    /// <summary>
    /// Gets the list of all companies
    /// </summary>
    /// <param name="companyParameters"></param>
    /// <returns>The companies list</returns>
    [HttpGet(Name = "GetCompanies")]
    [HttpHead]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] CompanyParameters companyParameters)
    {
        var linkParams = new
            CompanyLinkParameters(companyParameters, HttpContext);

        var result = await _service.CompanyService
            .GetAllAsync(linkParams, trackChanges: false);

        Response.Headers
          .Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

        return result.linkResponse.HasLinks ?
            Ok(result.linkResponse.LinkedEntities) :
            Ok(result.linkResponse.ShapedEntities);
    }


    [HttpGet("{id:guid}", Name = "CompanyById")]
    //[OutputCache(Duration = 60)]
    //[EnableRateLimiting("SpecificPolicy")] for certain endpoint
    //[DisableRateLimiting] for disabling the rate limiting for certain endpoint
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var result =
            await _service.CompanyService.GetAsync(id, trackChanges: false)!;

        return Ok(result);
    }


    /// <summary>
    /// Creates a newly created company
    /// </summary>
    /// <param name="company"></param>
    /// <returns>A newly created company</returns>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">If the item is null</response>
    /// <response code="422">If the model is invalid</response>
    [HttpPost(Name = "CreateCompany")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
    {
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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
    {
        await _service.CompanyService.UpdateAsync(id,
                                        company,
                                                    trackChanges: true);

        return NoContent();
    }
}
