using Asp.Versioning;
using Asp.Versioning.Conventions;
using Presentation.Controllers.V1.Authentication;
using Presentation.Controllers.V1.Sample;
using Presentation.Controllers.V2;

namespace Presentation.Helpers;

public static class VersioningHelper
{
    public static void ConfigureControllerVersions(IApiVersionConventionBuilder conventions)
    {
        // Sample:
        conventions.Controller<CompaniesController>()
            .HasApiVersion(new ApiVersion(1, 0));

        conventions.Controller<CompaniesV2Controller>()
            .HasDeprecatedApiVersion(new ApiVersion(2, 0));

        conventions.Controller<EmployeesController>()
            .HasApiVersion(new ApiVersion(1, 0));

        // Current project controllers:
        conventions.Controller<AuthenticationController>()
            .HasApiVersion(new ApiVersion(1, 0));

        conventions.Controller<RolesController>()
            .HasApiVersion(new ApiVersion(1, 0));

        conventions.Controller<TokenController>()
            .HasApiVersion(new ApiVersion(1, 0));

        conventions.Controller<UsersController>()
            .HasApiVersion(new ApiVersion(1, 0));
    }
}
