using Api.Utilities.CsvFormat.Sample.Company;
using Contracts.Base;
using Contracts.Logging;
using LoggerService.Logging;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Service.Base;
using Service.Contracts.Base;

namespace Api.Utilities.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .WithExposedHeaders("X-Pagination"));
        });


    public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options =>
        {

        });


    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();


    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();


    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();


    public static void ConfigureSqlContext(this IServiceCollection services,
                                                IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts =>
        opts.UseSqlServer(configuration
            .GetConnectionString("SqlConnection")));


    public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
        builder.AddMvcOptions(config =>
        config.OutputFormatters.Add(new CompanyCsvOutputFormatter()));


    
}
