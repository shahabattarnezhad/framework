using Entities.Models.Base;

namespace Entities.Models.Sample;

public class Employee : BaseEntity<Guid>
{
    public string? FullName { get; set; }

    public string? Position { get; set; }

    public int Age { get; set; }


    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
}
