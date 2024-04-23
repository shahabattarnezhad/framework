using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Sample.Employee;

public abstract record EmployeeForManipulationDto
{
    [Required(ErrorMessage = "The full name is required")]
    [MaxLength(100, ErrorMessage = "The maximum lenght characters is 100.")]
    public string? FullName { get; init; }


    [Required(ErrorMessage = "The position is required")]
    [MaxLength(50, ErrorMessage = "The maximum lenght characters is 50.")]
    public string? Position { get; init; }


    [Range(18, int.MaxValue,
        ErrorMessage = "Age is required and it can't be lower than 18")]
    public int Age { get; init; }
}
