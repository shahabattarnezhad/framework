using Api.Utilities.Links;
using Api.Utilities.Links.Authentication;
using Contracts.Links.Authentication;
using Contracts.Links.Sample;

namespace Api.Utilities.Extensions;

public static class LinksRegistration
{
    // Sample:
    public static void ConfigureEmployeeLinks(this IServiceCollection services) =>
        services.AddScoped<IEmployeeLinks, EmployeeLinks>();

    public static void ConfigureCompanyLinks(this IServiceCollection services) =>
        services.AddScoped<ICompanyLinks, CompanyLinks>();

    // Authentication:
    public static void ConfigureUserLinks(this IServiceCollection services) =>
        services.AddScoped<IUserLinks, UserLinks>();

    public static void ConfigureRoleLinks(this IServiceCollection services) =>
        services.AddScoped<IRoleLinks, RoleLinks>();

    // Register all of them:
    public static void ConfigureAllLinks(this IServiceCollection services)
    {
        services.ConfigureEmployeeLinks();
        services.ConfigureCompanyLinks();

        services.ConfigureUserLinks();
        services.ConfigureRoleLinks();
    }
}
