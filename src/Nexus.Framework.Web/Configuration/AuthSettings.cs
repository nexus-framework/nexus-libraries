namespace Nexus.Framework.Web.Configuration;

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