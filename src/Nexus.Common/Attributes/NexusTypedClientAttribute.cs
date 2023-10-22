namespace Nexus.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class NexusTypedClientAttribute<T> : Attribute, INexusTypedClientAttribute
{
    public string Name { get; }
    
    public NexusTypedClientAttribute(string name)
    {
        Name = name;
    }
}

public interface INexusTypedClientAttribute
{
    string Name { get; }
}