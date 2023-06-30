namespace Nexus.Framework.Web.Configuration;

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