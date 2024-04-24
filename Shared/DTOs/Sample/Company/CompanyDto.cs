using Shared.DTOs.Base;

namespace Shared.DTOs.Sample.Company;

public record CompanyDto : BaseEntityDto<Guid>
{
    public string? Name { get; init; }

    public string? Address { get; init; }
}
