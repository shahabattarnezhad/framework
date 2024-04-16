using Shared.DTOs.Base;

namespace Shared.DTOs.Sample.Employee;

public class EmployeeDto : BaseEntityDto<Guid>
{
    public string? FullName { get; set; }

    public string? Position { get; set; }

    public int Age { get; set; }
}
