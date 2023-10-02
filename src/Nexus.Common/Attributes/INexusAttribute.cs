namespace Nexus.Common.Attributes;

public interface INexusAttribute
{
    NexusServiceLifeTime Lifetime { get; }
}

public enum NexusServiceLifeTime
{
    Scoped = 1,
    Singleton = 2,
    Transient = 3,
}