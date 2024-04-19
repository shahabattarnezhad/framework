using Shared.DTOs.Sample.Employee;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Sample.Company;

public class CompanyForCreationDto
{
    [Required(ErrorMessage = "The name is required")]
    [MaxLength(50, ErrorMessage = "The maximum lenght characters is 50.")]
    public string? Name { get; set; }


    [Required(ErrorMessage = "The address is required")]
    [MaxLength(250, ErrorMessage = "The maximum lenght characters is 250.")]
    public string? Address { get; set; }


    public IEnumerable<EmployeeForCreationDto>? Employees { get; set; }
}
