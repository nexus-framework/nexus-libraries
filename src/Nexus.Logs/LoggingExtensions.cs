using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace Nexus.Logs;

/// <summary>
/// Provides extension methods for configuring core logging.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Adds core logging services to the provided <see cref="ILoggingBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to configure logging.</param>
    /// <param name="configuration">The configuration object.</param>
    public static void AddNexusLogging(this ILoggingBuilder builder, IConfiguration configuration)
    {
        SerilogSettings serilogSettings = new ();
        configuration.GetRequiredSection(nameof(SerilogSettings)).Bind(serilogSettings);

        Logger logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(serilogSettings.ElasticSearchSettings.Uri))
            {
                ModifyConnectionSettings = connectionConfiguration =>
                    connectionConfiguration
                        .BasicAuthentication(
                            serilogSettings.ElasticSearchSettings.Username, 
                            serilogSettings.ElasticSearchSettings.Password)
                        .ServerCertificateValidationCallback((a, b, c, d) => true),
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                AutoRegisterTemplate = true,
                BatchAction = ElasticOpType.Create,
                BatchPostingLimit = 20,
                IndexFormat = serilogSettings.ElasticSearchSettings.IndexFormat,
                CustomFormatter = new ElasticsearchJsonFormatter(),
            })
            .CreateLogger();

        builder.ClearProviders().AddSerilog(logger);
    }
}
