using Contracts.DataShaping;
using Service.DataShaping;
using Shared.DTOs.Authentication;
using Shared.DTOs.Sample.Company;
using Shared.DTOs.Sample.Employee;

namespace Api.Utilities.Extensions;

public static class DataShapingRegisteration
{
    // Sample:
    public static void ConfigureCompanyDataShaper(this IServiceCollection services) =>
        services.AddScoped<IDataShaper<CompanyDto, Guid>, DataShaper<CompanyDto, Guid>>();

    public static void ConfigureEmployeeDataShaper(this IServiceCollection services) =>
        services.AddScoped<IDataShaper<EmployeeDto, Guid>, DataShaper<EmployeeDto, Guid>>();

    // Authentication:
    public static void ConfigureUserDataShaper(this IServiceCollection services) =>
        services.AddScoped<IDataShaper<UserForDisplayDto, string>, DataShaper<UserForDisplayDto, string>>();

    public static void ConfigureRoleDataShaper(this IServiceCollection services) =>
        services.AddScoped<IDataShaper<RoleDto, string>, DataShaper<RoleDto, string>>();

    // Register all of them:
    public static void ConfigureAllDataShapers(this IServiceCollection services)
    {
        // Sample:
        services.ConfigureCompanyDataShaper();
        services.ConfigureEmployeeDataShaper();

        // Authentication:
        services.ConfigureUserDataShaper();
        services.ConfigureRoleDataShaper();
    }
}
