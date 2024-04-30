using Api.Utilities.CsvFormat.Sample.Company;
using Asp.Versioning;
using Contracts.Base;
using Contracts.Logging;
using Entities.Models.Authentication;
using LoggerService.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Presentation.Controllers.V1.Sample;
using Presentation.Controllers.V2;
using Repository.Base;
using Repository.Data;
using Service.Base;
using Service.Contracts.Base;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Entities.ConfigurationModels;

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


    public static void ConfigureResponseCaching(this IServiceCollection services) =>
        services.AddResponseCaching();


    public static void ConfigureOutputCaching(this IServiceCollection services) =>
        services.AddOutputCache(options =>
        {
            options.AddPolicy("120SecondsDuration", p =>
                     p.Expire(TimeSpan.FromSeconds(120)));
        });


    public static void ConfigureRateLimitingOptions(this IServiceCollection services)
    {
        services.AddRateLimiter(opt =>
        {
            opt.GlobalLimiter = PartitionedRateLimiter
               .Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter",
                partition => new FixedWindowRateLimiterOptions
                {

                    AutoReplenishment = true,
                    PermitLimit = 30,
                    QueueLimit = 2,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    Window = TimeSpan.FromSeconds(10)

                }));

            opt.AddPolicy("SpecificPolicy", context =>
                RateLimitPartition.GetFixedWindowLimiter("SpecificLimiter",
                partition => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 5,
                    Window = TimeSpan.FromSeconds(10)
                }));

            opt.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429;

                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter,
                                                    out var retryAfter))
                {
                    await context.HttpContext
                    .Response
                    .WriteAsync($"Too many requests. Please try again after {retryAfter.TotalSeconds} second(s).",
                    token);
                }
                else
                {
                    await context.HttpContext
                    .Response
                    .WriteAsync("Too many requests. Please try again later.",
                    token);
                }
            };
        });
    }


    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, IdentityRole>(option =>
        {
            option.Password.RequireDigit = true;
            option.Password.RequireLowercase = false;
            option.Password.RequireUppercase = false;
            option.Password.RequireNonAlphanumeric = false;
            option.Password.RequiredLength = 6;
            option.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
    }


    public static void ConfigureJWT(this IServiceCollection services,
                                         IConfiguration configuration)
    {
        var jwtConfiguration = new JwtConfiguration();

        configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

        var secretKey =
            Environment.GetEnvironmentVariable("SECRET");

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

            opt.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtConfiguration.ValidIssuer,
                    ValidAudience = jwtConfiguration.ValidAudience,

                    IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                };
            });
    }
}
