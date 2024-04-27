using Api.Utilities.Links;
using Contracts.Links.Sample;

namespace Api.Utilities.Extensions.Sample;

public static class LinksRegistration
{
    public static void ConfigureEmployeeLinks(this IServiceCollection services) =>
        services.AddScoped<IEmployeeLinks, EmployeeLinks>();


    public static void ConfigureCompanyLinks(this IServiceCollection services) =>
        services.AddScoped<ICompanyLinks, CompanyLinks>();
}
