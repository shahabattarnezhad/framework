using Shared.DTOs.Base;

namespace Shared.DTOs.Sample.Company;

public class CompanyDto : BaseEntityDto<Guid>
{
    public string? Name { get; set; }

    public string? Address { get; set; }
}
