using Shared.RequestFeatures.Base;

namespace Shared.RequestFeatures.Sample;

public class CompanyParameters : RequestParameters
{
    public CompanyParameters() => OrderBy = "name";

    public string? SearchTerm { get; set; }
}
