using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Base;

namespace Presentation.Controllers.V2;


[Route("api/companies")]
[ApiController]
public class CompaniesV2Controller : ControllerBase
{
    private readonly IServiceManager _service;
    public CompaniesV2Controller(IServiceManager service) => _service = service;


    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var result = 
            await _service.CompanyService.GetAllAsync(trackChanges: false);

        var companiesV2 = 
            result.Select(x => $"{x.Name} V2");
        
        return Ok(companiesV2);

        //return Ok(result);
    }
}
