namespace Nexus.Common.Attributes;

/// <summary>
/// Attribute used to annotate typed client classes used for Nexus.
/// </summary>
/// <typeparam name="T">The type of the client the attribute is applied to.</typeparam>
[AttributeUsage(AttributeTargets.Class)]
public class NexusTypedClientAttribute<T> : Attribute, INexusTypedClientAttribute
{
    /// <summary>
    /// Gets the name of the typed client.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Constructor that takes a name for the typed client.
    /// </summary>
    /// <param name="name">Name of the typed client.</param>
    public NexusTypedClientAttribute(string name)
    {
        Name = name;
    }
}

/// <summary>
/// Interface implemented by NexusTypedClientAttribute.
/// </summary>
public interface INexusTypedClientAttribute
{
    /// <summary>
    /// Gets the name of the typed client.
    /// </summary>
    string Name { get; }
}