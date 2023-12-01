using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Nexus.Common.Attributes;
using Nexus.Framework.Web.Configuration;
using Nexus.Framework.Web.Exceptions;
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
    private static void AddApiDocumentation(this IServiceCollection services, ApiDocumentationSettings settings, Assembly callingAssembly)
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

            if (callingAssembly != null)
            {
                string xmlFileName = $"{callingAssembly.GetName().Name}.xml";
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

    private static void AddNexusServices(this IServiceCollection services, Assembly assembly)
    {
        Type[] allTypes = assembly.GetTypes();
        IEnumerable<Type> genericNexusServiceTypes =
            allTypes.Where(t => t.GetCustomAttributes(typeof(NexusServiceAttribute<>), true).Length > 0);
        IEnumerable<Type> nexusServiceTypes =
            allTypes.Where(t => t.GetCustomAttributes(typeof(NexusServiceAttribute), true).Length > 0);
        
        // INexusService<T>
        foreach (Type type in genericNexusServiceTypes)
        {
            Attribute attribute = type.GetCustomAttribute(typeof(NexusServiceAttribute<>))!;
            Type genericType = attribute.GetType().GetGenericArguments()[0];
            INexusServiceAttribute nexusServiceAttribute = (attribute as INexusServiceAttribute)!;

            switch (nexusServiceAttribute.Lifetime)
            {
                case NexusServiceLifeTime.Scoped:
                    services.AddScoped( genericType, type);
                    break;
                case NexusServiceLifeTime.Singleton:
                    services.AddSingleton(genericType, type);
                    break;
                case NexusServiceLifeTime.Transient:
                    services.AddTransient(genericType, type);
                    break;
            }
        }
        
        // INexusService
        foreach (Type type in nexusServiceTypes)
        {
            Attribute attribute = type.GetCustomAttribute(typeof(NexusServiceAttribute))!;
            INexusServiceAttribute nexusServiceAttribute = (attribute as INexusServiceAttribute)!;

            switch (nexusServiceAttribute.Lifetime)
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

    private static void AddNexusTypedClients(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {
        Type[] allTypes = assembly.GetTypes();
        IEnumerable<Type> nexusTypedClientTypes =
            allTypes.Where(t => t.GetCustomAttributes(typeof(NexusTypedClientAttribute<>), true).Length > 0);
        
        foreach (Type implementationType in nexusTypedClientTypes)
        {
            Attribute attribute = implementationType.GetCustomAttribute(typeof(NexusTypedClientAttribute<>))!;
            Type interfaceType = attribute.GetType().GetGenericArguments()[0];
            INexusTypedClientAttribute nexusTypedClientAttribute = (attribute as INexusTypedClientAttribute)!;

            services.AddNexusTypedClient(configuration, interfaceType, implementationType, nexusTypedClientAttribute.Name);
        }
    }

    private static void AddNexusTypedClient(
        this IServiceCollection services,
        IConfiguration configuration,
        Type interfaceType,
        Type implementationType,
        string clientName)
    {
        Type extensionMethodClassType = typeof(HttpClientBuilderExtensions);
        MethodInfo[] extensionMethods = extensionMethodClassType.GetMethods(BindingFlags.Static | BindingFlags.Public);
        MethodInfo? addTypedClientMethod = extensionMethods.FirstOrDefault(method => method.Name == "AddTypedClient" && method.GetGenericArguments().Length == 2);

        if (addTypedClientMethod == null)
        {
            throw new NexusTypedClientException(NexusTypedClientException.UnableToRegisterTypedClient);
        }
        
        FrameworkSettings settings = new ();
        configuration.GetRequiredSection(nameof(FrameworkSettings)).Bind(settings);
        
        IHttpClientBuilder clientBuilder = services.AddHttpClient(clientName);
        if (settings.Discovery is { Enable: true })
        {
            clientBuilder.AddServiceDiscovery();
        }

        clientBuilder
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
            });
        
        MethodInfo genericMethod = addTypedClientMethod.MakeGenericMethod(interfaceType, implementationType);
        object[] parameters = { clientBuilder };
        genericMethod.Invoke(null, parameters);
    }
    
    /// <summary>
    /// Adds core services and features to the service collection based on the specified configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddWebFramework(this IServiceCollection services, IConfiguration configuration, Assembly callingAssembly)
    {
        FrameworkSettings settings = new ();
        configuration.GetRequiredSection(nameof(FrameworkSettings)).Bind(settings);

        if (settings.Swagger is { Enable: true })
        {
            services.AddApiDocumentation(settings.Swagger, callingAssembly);
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
        
        services.AddNexusServices(typeof(DependencyInjectionExtensions).Assembly);
        services.AddNexusServices(callingAssembly);
        
        services.AddNexusTypedClients(configuration, callingAssembly);
        services.AddNexusPersistence(configuration, callingAssembly);
        
        services.AddAutoMapper(callingAssembly);
        services.AddValidatorsFromAssembly(callingAssembly);
    }
}