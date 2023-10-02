using Nexus.Common.Abstractions;

namespace Nexus.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute<T> : Attribute
    where T : INexusModule
{
}