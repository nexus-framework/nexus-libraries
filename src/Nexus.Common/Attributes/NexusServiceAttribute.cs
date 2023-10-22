using Nexus.Common.Abstractions;

namespace Nexus.Common.Attributes;

#pragma warning disable S2326
[AttributeUsage(AttributeTargets.Class)]
public class NexusServiceAttribute<T> : Attribute, INexusServiceAttribute
    where T : INexusService
{
    public NexusServiceLifeTime Lifetime { get; }

    public NexusServiceAttribute(NexusServiceLifeTime lifeTime)
    {
        Lifetime = lifeTime;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class NexusServiceAttribute : Attribute, INexusServiceAttribute
{
    public NexusServiceLifeTime Lifetime { get; }

    public NexusServiceAttribute(NexusServiceLifeTime lifetime)
    {
        Lifetime = lifetime;
    }
}
#pragma warning restore S2326

public interface INexusServiceAttribute
{
    NexusServiceLifeTime Lifetime { get; }
}

public enum NexusServiceLifeTime
{
    Scoped = 1,
    Singleton = 2,
    Transient = 3,
}