using Shared.DTOs.Base;

namespace Shared.DTOs.Sample.Company;

public class CompanyDto : BaseEntityDto<Guid>
{
    public string? Name { get; init; }

    public string? Address { get; init; }
}
