using Microsoft.Extensions.Configuration;
using Nexus.Configuration;

namespace Nexus.Framework.Web.Configuration;

/// <summary>
/// Extension methods for configuring the Nexus Web Framework 
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Configures the Nexus Web Framework
    /// </summary>
    /// <param name="configuration"></param>
    public static void ConfigureWebFramework(this ConfigurationManager configuration)
    {
        configuration.AddNexusConfiguration();
        
        // Settings for consul kv
        bool configureKv = configuration.GetValue<bool>("FrameworkSettings:Discovery:Enable");
        if (configureKv)
        {
            ConsulKVSettings consulKvSettings = new ();
            configuration.GetRequiredSection("ConsulKV").Bind(consulKvSettings);
            configuration.AddConsulKv(consulKvSettings);
        }
    }
}