using Shared.RequestFeatures.Base;

namespace Shared.RequestFeatures.Authentication;

public class UserParameters : RequestParameters
{
    public UserParameters() => OrderBy = "userName";

    public string? SearchTerm { get; set; }
}
