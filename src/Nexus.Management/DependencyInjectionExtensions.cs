using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Endpoint.Refresh;

namespace Nexus.Management;

/// <summary>
/// A static class that provides extension methods for adding core actuators to the service collection.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds the health, info, and refresh actuators to the service collection and activates the actuator endpoints.
    /// </summary>
    /// <param name="services">The service collection to add the actuators to.</param>
    /// <param name="configuration">The configuration to use for the actuators.</param>
    public static void AddNexusActuators(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthActuator(configuration);
        services.AddInfoActuator(configuration);
        services.AddHealthChecks();
        services.AddRefreshActuator();
        services.ActivateActuatorEndpoints();
    }
}
