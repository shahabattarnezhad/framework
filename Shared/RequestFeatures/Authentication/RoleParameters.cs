using Shared.RequestFeatures.Base;

namespace Shared.RequestFeatures.Authentication;

public class RoleParameters : RequestParameters
{
    public RoleParameters() => OrderBy = "name";

    public string? SearchTerm { get; set; }
}
