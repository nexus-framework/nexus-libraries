namespace Nexus.Framework.Web.Configuration;

/// <summary>
/// Represents the framework settings for the application.
/// </summary>
public class FrameworkSettings
{
    /// <summary>
    /// Gets or sets the authentication settings.
    /// </summary>
    public AuthSettings? Auth { get; set; }

    /// <summary>
    /// Gets or sets the API documentation settings.
    /// </summary>
    public ApiDocumentationSettings? Swagger { get; set; }

    /// <summary>
    /// Gets or sets the API controller settings.
    /// </summary>
    public ApiControllerSettings? ApiControllers { get; set; }
    
    /// <summary>
    /// Gets or sets the telemetry settings.
    /// </summary>
    public TelemetrySettings? Telemetry { get; set; }

    /// <summary>
    /// Gets or sets the management settings.
    /// </summary>
    public ManagementSettings? Management { get; set; }

    /// <summary>
    /// Gets or sets the discovery settings.
    /// </summary>
    public DiscoverySettings? Discovery { get; set; }
    
    /// <summary>
    /// Gets or sets the logging settings
    /// </summary>
    public LoggingSettings? Logging { get; set; }
}