using Nexus.Common.Abstractions;

namespace Nexus.Common.Attributes;

#pragma warning disable S2326
[AttributeUsage(AttributeTargets.Class)]
public class NexusServiceAttribute<T> : Attribute, INexusAttribute
    where T : INexusService
{
    public NexusServiceLifeTime Lifetime { get; }

    public NexusServiceAttribute(NexusServiceLifeTime lifeTime)
    {
        Lifetime = lifeTime;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class NexusServiceAttribute : Attribute, INexusAttribute
{
    public NexusServiceLifeTime Lifetime { get; }

    public NexusServiceAttribute(NexusServiceLifeTime lifetime)
    {
        Lifetime = lifetime;
    }
}
#pragma warning restore S2326