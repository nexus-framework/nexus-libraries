using Nexus.Common.Abstractions;

namespace Nexus.Common.Attributes;

#pragma warning disable S2326
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute<T> : Attribute
    where T : INexusModule
{
}
#pragma warning restore S2326