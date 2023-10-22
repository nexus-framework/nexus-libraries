namespace Nexus.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class NexusMeterAttribute : Attribute, INexusMeterAttribute
{
    public string Name { get; }
    public NexusMeterAttribute(string name)
    {
        Name = name;
    }
}

public interface INexusMeterAttribute
{
    string Name { get; }
}
