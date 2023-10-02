using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Nexus.Common.Attributes;
using Nexus.Framework.Web.Configuration;
using Nexus.Framework.Web.Filters;
using Nexus.Management;
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

    private static void AddNexusServices(this IServiceCollection services)
    {
        Type[] allTypes = Assembly.GetCallingAssembly().GetTypes();
        
        IEnumerable<Type> genericNexusServiceTypes =
            allTypes.Where(t => t.GetCustomAttributes(typeof(NexusServiceAttribute<>), true).Length > 0);
        IEnumerable<Type> nexusServiceTypes =
            allTypes.Where(t => t.GetCustomAttributes(typeof(NexusServiceAttribute), true).Length > 0);
        
        // INexusService<T>
        foreach (Type type in genericNexusServiceTypes)
        {
            Attribute attribute = type.GetCustomAttribute(typeof(NexusServiceAttribute<>))!;
            Type genericType = attribute.GetType().GetGenericArguments()[0];
            INexusAttribute nexusAttribute = (attribute as INexusAttribute)!;

            switch (nexusAttribute.Lifetime)
            {
                case NexusServiceLifeTime.Scoped:
                    services.AddScoped(type, genericType);
                    break;
                case NexusServiceLifeTime.Singleton:
                    services.AddSingleton(type, genericType);
                    break;
                case NexusServiceLifeTime.Transient:
                    services.AddTransient(type, genericType);
                    break;
            }
        }
        
        // INexusService
        foreach (Type type in nexusServiceTypes)
        {
            Attribute attribute = type.GetCustomAttribute(typeof(NexusServiceAttribute<>))!;
            INexusAttribute nexusAttribute = (attribute as INexusAttribute)!;

            switch (nexusAttribute.Lifetime)
            {
                case NexusServiceLifeTime.Scoped:
                    services.AddScoped(type);
                    break;
                case NexusServiceLifeTime.Singleton:
                    services.AddSingleton(type);
                    break;
                case NexusServiceLifeTime.Transient:
                    services.AddTransient(type);
                    break;
            }
        }
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
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
        });
        
        services.AddNexusServices();
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