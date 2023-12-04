namespace Nexus.Common.Attributes;

/// <summary>
/// Represents a custom attribute for a Nexus Meter. This class cannot be inherited.
/// </summary>
/// <remarks>
/// This class is for marking user-defined classes with additional metadata.
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public class NexusMeterAttribute : Attribute, INexusMeterAttribute
{
    /// <summary>
    /// Gets the name of the Nexus Meter.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="NexusMeterAttribute"/> class with the specified meter name.
    /// </summary>
    /// <param name="name">The name of the Nexus Meter.</param>
    public NexusMeterAttribute(string name)
    {
        Name = name;
    }
}

/// <summary>
/// Represents an attribute of a Nexus Meter.
/// </summary>
public interface INexusMeterAttribute
{
    /// <summary>
    /// Gets the name of the attribute.
    /// </summary>
    string Name { get; }
}
