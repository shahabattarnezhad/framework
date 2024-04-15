﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Base;

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
        try
        {
            var results =
                _service.CompanyService.GetAll(trackChanges: false);

            return Ok(results);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }
}
