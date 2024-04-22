using Shared.DTOs.Sample.Employee;

namespace Shared.DTOs.Sample.Company;

public class CompanyForUpdateDto
{
    public string? Name { get; set; }

    public string? Address { get; set; }


    public ICollection<EmployeeForCreationDto>? Employees { get; set; }
}
