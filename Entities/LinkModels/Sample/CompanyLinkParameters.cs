using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures.Sample;

namespace Entities.LinkModels.Sample;

public record CompanyLinkParameters(CompanyParameters CompanyParameters,
                                     HttpContext Context);
