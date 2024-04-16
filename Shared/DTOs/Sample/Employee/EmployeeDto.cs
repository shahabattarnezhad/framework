using Shared.DTOs.Base;

namespace Shared.DTOs.Sample.Employee;

public class EmployeeDto : BaseEntityDto<Guid>
{
    public string? FullName { get; init; }

    public string? Position { get; init; }

    public int Age { get; init; }
}
