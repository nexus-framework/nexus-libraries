using Nexus.Common.Abstractions;

namespace Nexus.Common.Attributes;
#pragma warning disable S2326

/// <summary>
/// Attribute to identify Nexus Services with a specified life time.
/// </summary>
/// <typeparam name="T">Type of the service that needs to be registered. It should be derived from INexusService.</typeparam>
[AttributeUsage(AttributeTargets.Class)]
public class NexusServiceAttribute<T> : Attribute, INexusServiceAttribute
    where T : INexusService
{
    /// <summary>
    /// Gets the life time of the service.
    /// </summary>
    public NexusServiceLifeTime Lifetime { get; }

    /// <summary>
    /// Initialize a new instance of NexusServiceAttribute with specified service lifetime
    /// </summary>
    /// <param name="lifeTime">Life time of the service</param>
    public NexusServiceAttribute(NexusServiceLifeTime lifeTime)
    {
        Lifetime = lifeTime;
    }
}

/// <summary>
/// Attribute to identify Nexus Services with a specified life time.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class NexusServiceAttribute : Attribute, INexusServiceAttribute
{
    /// <summary>
    /// Gets the life time of the service.
    /// </summary>
    public NexusServiceLifeTime Lifetime { get; }

    /// <summary>
    /// Initialize a new instance of NexusServiceAttribute with specified service lifetime
    /// </summary>
    /// <param name="lifetime">Life time of the service</param>
    public NexusServiceAttribute(NexusServiceLifeTime lifetime)
    {
        Lifetime = lifetime;
    }
}
#pragma warning restore S2326

/// <summary>
/// Interface for attributes that identify Nexus Services.
/// </summary>
public interface INexusServiceAttribute
{
    /// <summary>
    /// Gets the life time of the service.
    /// </summary>
    NexusServiceLifeTime Lifetime { get; }
}

/// <summary>
/// Enum to identify the life time of a service.
/// </summary>
public enum NexusServiceLifeTime
{
    /// <summary>
    /// Instance is created once per request within the scope.
    /// </summary>
    Scoped = 1,

    /// <summary>
    /// Instance is created once and re-used across all requests.
    /// </summary>
    Singleton = 2,

    /// <summary>
    /// Instance is created every time a new instance is requested.
    /// </summary>
    Transient = 3,
}