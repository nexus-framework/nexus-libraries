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
    /// <param name="key">The key of the key-value pair to retrieve.</param>
    /// <param name="url">The url to Consul KV server.</param>
    /// <param name="token">The token to access the key-value pair.</param>
    private static void AddConsulKv(this ConfigurationManager configuration, string key, string url, string token)
    {
        configuration.AddConsul(key, options =>
        {
            options.ConsulConfigurationOptions = config =>
            {
                config.Address = new Uri(url);
                config.Token = token;
            };

            options.Optional = false;
            options.ReloadOnChange = true;
        });        
    }
    
    /// <summary>
    /// Adds Consul Key-Value (KV) configuration to the ConfigurationManager.
    /// </summary>
    /// <param name="configuration">The ConfigurationManager.</param>
    /// <param name="settings">The ConsulKVSettings containing the configuration options.</param>
    public static void AddConsulKv(this ConfigurationManager configuration, ConsulKVSettings settings)
    {
        configuration.AddConsulKv(settings.Key, settings.Url, settings.Token);
        
        if (settings.AddGlobalConfig)
        {
            configuration.AddConsulKv(settings.GlobalConfigKey, settings.Url, settings.Token);
        }
    }

    /// <summary>
    /// Adds core configuration settings to the ConfigurationManager.
    /// </summary>
    /// <param name="configuration">The ConfigurationManager.</param>
    public static void AddCoreConfiguration(this ConfigurationManager configuration)
    {
        configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.Global.json", optional: true)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables();
    }
}
