using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures.Authentication;

namespace Entities.LinkModels.Authentication;

public record RoleLinkParameters(RoleParameters RoleParameters, HttpContext Context);
