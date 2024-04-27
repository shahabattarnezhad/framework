﻿using Api.Utilities.CsvFormat.Sample.Company;
using Asp.Versioning;
using Contracts.Base;
using Contracts.Logging;
using LoggerService.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Presentation.Controllers.V1.Sample;
using Presentation.Controllers.V2;
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


    public static void AddCustomMediaTypes(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            var systemTextJsonOutputFormatter =
            config.OutputFormatters
                  .OfType<SystemTextJsonOutputFormatter>()?
                  .FirstOrDefault();

            if (systemTextJsonOutputFormatter != null)
            {
                systemTextJsonOutputFormatter.SupportedMediaTypes
                .Add("application/vnd.framework.hateoas+json");

                systemTextJsonOutputFormatter.SupportedMediaTypes
                .Add("application/vnd.framework.apiroot+json");
            }

            var xmlOutputFormatter =
            config.OutputFormatters
                  .OfType<XmlDataContractSerializerOutputFormatter>()?
                  .FirstOrDefault();

            if (xmlOutputFormatter != null)
            {
                xmlOutputFormatter.SupportedMediaTypes
                .Add("application/vnd.framework.hateoas+xml");

                xmlOutputFormatter.SupportedMediaTypes
                .Add("application/vnd.framework.apiroot+xml");
            }

        });
    }


    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {

            opt.ReportApiVersions = true;
            
            opt.AssumeDefaultVersionWhenUnspecified = true;
            
            opt.DefaultApiVersion = new ApiVersion(1, 0);

            opt.ApiVersionReader = new HeaderApiVersionReader("api-version");

        })
        .AddMvc(opt =>
        { 
            opt.Conventions.Controller<CompaniesController>()
            .HasApiVersion(new ApiVersion(1, 0));
            
            opt.Conventions.Controller<CompaniesV2Controller>()
            .HasDeprecatedApiVersion(new ApiVersion(2, 0));

            opt.Conventions.Controller<EmployeesController>()
            .HasApiVersion(new ApiVersion(1, 0));
        });
    }
}
