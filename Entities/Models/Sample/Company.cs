using Entities.Models.Base;

namespace Entities.Models.Sample;

public class Company : BaseEntity<Guid>
{
    public string? Name { get; set; }

    public string? Address { get; set; }


    public ICollection<Employee>? Employees { get; set; }
}
