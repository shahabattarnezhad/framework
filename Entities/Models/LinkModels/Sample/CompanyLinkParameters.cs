using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures.Sample;

namespace Entities.Models.LinkModels.Sample;

public record CompanyLinkParameters(CompanyParameters CompanyParameters,
                                     HttpContext Context);
