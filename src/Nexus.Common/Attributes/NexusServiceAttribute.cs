using Nexus.Common.Abstractions;

namespace Nexus.Common.Attributes;

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