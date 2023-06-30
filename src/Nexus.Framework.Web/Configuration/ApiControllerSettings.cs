namespace Nexus.Framework.Web.Configuration;

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