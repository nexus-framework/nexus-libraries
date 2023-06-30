using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nexus.Framework.Web.Configuration;
using Nexus.Logs;

namespace Nexus.Framework.Web.Logging;

/// <summary>
/// Extension methods for Nexus Web Framework logging
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Configures Nexus Web Framework logging
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to configure logging.</param>
    /// <param name="configuration">The configuration object.</param>
    public static void ConfigureWebFrameworkLogging(this ILoggingBuilder builder, IConfiguration configuration)
    {
        FrameworkSettings settings = new ();
        configuration.GetRequiredSection(nameof(FrameworkSettings)).Bind(settings);
        if (settings.Logging is { Enable: true })
        {
            builder.AddCoreLogging(configuration);
        }
    }
}