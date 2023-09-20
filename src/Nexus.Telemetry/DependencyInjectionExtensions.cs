using Microsoft.Extensions.Configuration;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Nexus.Telemetry;
using OpenTelemetry;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for configuring dependency injection for core telemetry.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds core telemetry services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration object.</param>
    public static void AddNexusTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        TelemetrySettings telemetrySettings = new ();
        configuration.GetRequiredSection(nameof(TelemetrySettings)).Bind(telemetrySettings);

        services
            .AddOpenTelemetry()
            .WithTracing(builder =>
                {
                    builder
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .ConfigureResource(options =>
                        {
                            options.AddService(
                                telemetrySettings.ServiceName,
                                serviceVersion: telemetrySettings.ServiceVersion,
                                autoGenerateServiceInstanceId: true);
                        })
                        .AddOtlpExporter(options => { options.Endpoint = new Uri(telemetrySettings.Endpoint); });

                    if (telemetrySettings.EnableConsoleExporter)
                    {
                        builder.AddConsoleExporter();
                    }

                    if (telemetrySettings.EnableAlwaysOnSampler)
                    {
                        builder.SetSampler<AlwaysOnSampler>();
                    }
                    else
                    {
                        builder.SetSampler(new TraceIdRatioBasedSampler(telemetrySettings.SampleProbability));
                    }
                }
            )
            .WithMetrics(builder =>
            {
                builder
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddPrometheusExporter();
            });
    }

    /// <summary>
    /// Adds Nexus meters to the OpenTelemetry configuration for monitoring purposes.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the meters to.</param>
    /// <param name="serviceName">The name of the service.</param>
    /// <param name="meterNames">An array of meter names to be added.</param>
    public static void AddNexusMeters(this IServiceCollection services, string serviceName, string[] meterNames)
    {
        OpenTelemetryBuilder telemetryBuilder = services.AddOpenTelemetry()
            .ConfigureResource(options => options.AddService(serviceName));
        
        if (meterNames.Length > 0)
        {
            telemetryBuilder.WithMetrics(builder =>
            {
                builder.AddMeter(meterNames);
            });
        }
    }
}
