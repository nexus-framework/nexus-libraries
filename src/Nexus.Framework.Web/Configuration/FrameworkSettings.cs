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
}

/// <summary>
/// Represents the authentication settings.
/// </summary>
public class AuthSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether authentication is enabled.
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// Gets or sets the resource name.
    /// </summary>
    public string ResourceName { get; set; } = string.Empty;
}

/// <summary>
/// Represents the API documentation settings.
/// </summary>
public class ApiDocumentationSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether API documentation is enabled.
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// Gets or sets the API documentation version.
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// Gets or sets the title of the API documentation.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the API documentation.
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// Represents the API controller settings.
/// </summary>
public class ApiControllerSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether API controllers are enabled.
    /// </summary>
    public bool Enable { get; set; }
    
    /// <summary>
    /// Gets or sets the filter settings for API controllers.
    /// </summary>
    public FilterSettings? Filters { get; set; }
}

/// <summary>
/// Represents the filter settings.
/// </summary>
public class FilterSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether action logging is enabled.
    /// </summary>
    public bool EnableActionLogging { get; set; }
}

/// <summary>
/// Represents the management settings.
/// </summary>
public class ManagementSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether management features are enabled.
    /// </summary>
    public bool Enable { get; set; }
}

/// <summary>
/// Represents the discovery settings.
/// </summary>
public class DiscoverySettings
{
    /// <summary>
    /// Gets or sets a value indicating whether service discovery is enabled.
    /// </summary>
    public bool Enable { get; set; }
}

/// <summary>
/// Represents the telemetry settings.
/// </summary>
public class TelemetrySettings
{
    /// <summary>
    /// Gets or sets a value indicating whether telemetry is enabled.
    /// </summary>
    public bool Enable { get; set; }
}
