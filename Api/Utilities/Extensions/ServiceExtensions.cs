using Api.Utilities.CsvFormat.Sample.Company;
using Asp.Versioning;
using Contracts.Base;
using Contracts.Logging;
using Entities.ConfigurationModels;
using Entities.Models.Authentication;
using LoggerService.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Base;
using Presentation.Helpers;
using Repository.Base;
using Repository.Data;
using Service.Attribute;
using Service.Base;
using Service.Contracts.Base;
using Service.Contracts.Interfaces.Authentication;
using Service.Services.Authentication;
using Shared.Constants;
using System.Text;
using System.Threading.RateLimiting;

namespace Api.Utilities.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials()
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


    public static void ConfigureFileService(this IServiceCollection services) =>
        services.AddScoped<IFileService, FileService>();


    public static void ConfigureProfileImageService(this IServiceCollection services) =>
        services.AddScoped<IUserProfileImageService, UserProfileImageService>();


    public static void ConfigureCacheInvalidationService(this IServiceCollection services) =>
    services.AddScoped<ICacheInvalidationService, CacheInvalidationService>();

    public static void ConfigurePermissionHandlerService(this IServiceCollection services) =>
    services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();


    public static void ConfigureRefreshCacheService(this IServiceCollection services) =>
    services.AddScoped(typeof(ICacheRefresherService<,>), typeof(CacheRefresherService<,>));


    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
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
            VersioningHelper.ConfigureControllerVersions(opt.Conventions);
        });
    }


    public static void ConfigureResponseCaching(this IServiceCollection services) =>
        services.AddResponseCaching();


    public static void ConfigureOutputCaching(this IServiceCollection services) =>
        services.AddOutputCache(options =>
        {
            // Sample: 
            options.AddPolicy("120SecondsDuration", p =>
            {
                p.Expire(TimeSpan.FromSeconds(120)).Tag("DefaultCache");
            });

            options.AddEntityCachePolicies<Guid>(
                entityName: "Company",
                listDuration: TimeSpan.FromMinutes(5),
                detailDuration: TimeSpan.FromMinutes(10));

            options.AddEntityCachePolicies<Guid>(
                entityName: "Employee",
                listDuration: TimeSpan.FromMinutes(5),
                detailDuration: TimeSpan.FromMinutes(10));
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
        var builder = services.AddIdentity<User, Role>(option =>
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

    public static void ConfigureAuthorization(this IServiceCollection services) =>
        services.AddAuthorization(options =>
        {
            foreach (var permission in PermissionConstants.AllPermissions)
            {
                options.AddPolicy(permission.Name, policy =>
                policy.Requirements.Add(new OperationAuthorizationRequirement { Name = permission.Name }));
            }
        });


    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
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

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                        if (!string.IsNullOrEmpty(accessToken))
                            ctx.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });
    }


    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));


    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Framework API",
                Version = "v1",
                Description = "Framework API by Comet",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Shahab Attarnejad",
                    Email = "atarnezhad@gmail.com",
                    Url = new Uri("https://www.atarnezhad.com/"),
                },
                License = new OpenApiLicense
                {
                    Name = "Framework API LICX",
                    Url = new Uri("https://example.com/license"),
                }
            });

            s.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = "Framework API",
                Version = "v2"
            });

            var xmlFile =
                $"{typeof(AssemblyReference).Assembly.GetName().Name}.xml";

            var xmlPath =
                Path.Combine(AppContext.BaseDirectory, xmlFile);

            s.IncludeXmlComments(xmlPath);

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },

                        Name = "Bearer",
                    },

                    new List<string>()
                }
            });
        });
    }
}
