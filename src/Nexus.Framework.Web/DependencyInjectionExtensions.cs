using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Nexus.Framework.Web.Configuration;
using Nexus.Framework.Web.Filters;
using Nexus.Framework.Web.Services;
using Nexus.Management;
using Nexus.Persistence.Auditing;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for configuring dependency injection in a web framework.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds API documentation using Swagger to the service collection based on the specified settings.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="settings">The API documentation settings.</param>
    private static void AddApiDocumentation(this IServiceCollection services, ApiDocumentationSettings settings)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = settings.Version,
                Title = settings.Title,
                Description = settings.Description,
            });

            Assembly? entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly != null)
            {
                string xmlFileName = $"{entryAssembly.GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
            }

            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme.",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearer",
                        },
                    },
                    Array.Empty<string>()
                },
            });
        });
    }

    /// <summary>
    /// Adds core services and features to the service collection based on the specified configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddWebFramework(this IServiceCollection services, IConfiguration configuration)
    {
        FrameworkSettings settings = new ();
        configuration.GetRequiredSection(nameof(FrameworkSettings)).Bind(settings);

        if (settings.Swagger is { Enable: true })
        {
            services.AddApiDocumentation(settings.Swagger);
        }

        if (settings.ApiControllers is { Enable: true })
        {
            services.AddControllers(options =>
            {
                if (settings.ApiControllers.Filters is { EnableActionLogging: true })
                {
                    options.Filters.Add<LoggingFilter>();
                }
            });
        }
        
        if (settings.Telemetry is { Enable: true })
        {
            services.AddNexusTelemetry(configuration);
        }

        if (settings.Management is { Enable: true })
        {
            services.AddNexusActuators(configuration);
        }

        if (settings.Discovery is { Enable: true })
        {
            services.AddDiscoveryClient(configuration);
        }

        if (settings.Auth is { Enable: true })
        {
            services.AddNexusAuth(configuration, settings.Auth.ResourceName);
        }

        services.AddHttpContextAccessor();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IDateTime, DateTimeService>();
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
        });
    }

    /// <summary>
    /// Adds a Nexus typed HTTP client to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TClient">The type of the client interface.</typeparam>
    /// <typeparam name="TImplementation">The type of the client implementation.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the client to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> used to retrieve framework settings.</param>
    /// <param name="clientName">The name of the HTTP client to configure.</param>
    public static void AddNexusTypedClient<TClient, TImplementation>(this IServiceCollection services, IConfiguration configuration, string clientName)
        where TClient : class
        where TImplementation : class, TClient
    {
        FrameworkSettings settings = new ();
        configuration.GetRequiredSection(nameof(FrameworkSettings)).Bind(settings);
        
        IHttpClientBuilder clientBuilder = services.AddHttpClient(clientName);
        if (settings.Discovery is { Enable: true })
        {
            clientBuilder.AddServiceDiscovery();
        }
        
        clientBuilder
            .AddServiceDiscovery()
            .ConfigureHttpClient((serviceProvider, options) =>
            {
                IHttpContextAccessor httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

                if (httpContextAccessor.HttpContext == null)
                {
                    return;
                }

                string? bearerToken = httpContextAccessor.HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault(h =>
                        !string.IsNullOrEmpty(h) &&
                        h.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(bearerToken))
                {
                    options.DefaultRequestHeaders.Add("Authorization", bearerToken);
                }
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
                };
            })
            .AddTypedClient<TClient, TImplementation>();
    }
}