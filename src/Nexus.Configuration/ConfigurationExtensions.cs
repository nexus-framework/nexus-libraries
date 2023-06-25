using Microsoft.Extensions.Configuration;
using Winton.Extensions.Configuration.Consul;

namespace Nexus.Configuration;

/// <summary>
/// Provides extension methods for configuring core configuration settings.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Adds Consul Key-Value (KV) configuration to the ConfigurationManager.
    /// </summary>
    /// <param name="configuration">The ConfigurationManager.</param>
    /// <param name="settings">The ConsulKVSettings containing the configuration options.</param>
    private static void AddConsulKv(this ConfigurationManager configuration, ConsulKVSettings settings)
    {
        configuration.AddConsul(settings.Key, options =>
        {
            options.ConsulConfigurationOptions = config =>
            {
                config.Address = new Uri(settings.Url);
                config.Token = settings.Token;
            };

            options.Optional = false;
            options.ReloadOnChange = true;
        });
    }

    /// <summary>
    /// Adds core configuration settings to the ConfigurationManager.
    /// </summary>
    /// <param name="configuration">The ConfigurationManager.</param>
    public static void AddCoreConfiguration(this ConfigurationManager configuration)
    {
        configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables();

        // Settings for consul kv
        ConsulKVSettings consulKvSettings = new ();
        configuration.GetRequiredSection("ConsulKV").Bind(consulKvSettings);
        configuration.AddConsulKv(consulKvSettings);
    }
}
