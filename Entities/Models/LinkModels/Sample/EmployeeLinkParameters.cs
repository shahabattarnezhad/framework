using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures.Sample;

namespace Entities.Models.LinkModels.Sample;

public record EmployeeLinkParameters(EmployeeParameters EmployeeParameters,
                                     HttpContext Context);
