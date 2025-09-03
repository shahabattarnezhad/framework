using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures.Authentication;

namespace Entities.LinkModels.Authentication;

public record UserLinkParameters(UserParameters UserParameters, HttpContext Context);
