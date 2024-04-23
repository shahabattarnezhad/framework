using Shared.DTOs.Sample.Employee;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Sample.Company;

public abstract record CompanyForManipulationDto
{
    [Required(ErrorMessage = "The name is required")]
    [MaxLength(50, ErrorMessage = "The maximum lenght characters is 50.")]
    public string? Name { get; init; }


    [Required(ErrorMessage = "The address is required")]
    [MaxLength(250, ErrorMessage = "The maximum lenght characters is 250.")]
    public string? Address { get; init; }


    public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
}
