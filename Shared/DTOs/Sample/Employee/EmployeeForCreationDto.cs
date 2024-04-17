using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Sample.Employee;

public class EmployeeForCreationDto
{
    [Required(ErrorMessage = "The full name is required")]
    [MaxLength(100, ErrorMessage = "The maximum lenght characters is 100.")]
    public string? FullName { get; set; }


    [Required(ErrorMessage = "The position is required")]
    [MaxLength(50, ErrorMessage = "The maximum lenght characters is 50.")]
    public string? Position { get; set; }


    [Required(ErrorMessage = "The age is required")]
    public int Age { get; set; }
}
