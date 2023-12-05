using System.Reflection;
using Nexus.Common.Attributes;

namespace Nexus.Common.UnitTests.Attributes;

[ExcludeFromCodeCoverage]
public class NexusTypedClientAttributeTests
{
    [Fact]
    public void Constructor_SetsNameProperty()
    {
        // Arrange
        const string expectedName = "custom-name";

        // Act
        NexusTypedClientAttribute<string>? attribute = new (expectedName);

        // Assert
        Assert.Equal(expectedName, attribute.Name);
    }

    [Fact]
    public void HasCorrectAttributeUsage()
    {
        // Arrange
        Type? type = typeof(NexusTypedClientAttribute<>);

        // Act
        AttributeUsageAttribute? attributeUsage = type.GetCustomAttribute<AttributeUsageAttribute>();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.True(attributeUsage.AllowMultiple == false);
        Assert.True(attributeUsage.Inherited == true);
        Assert.Equal(AttributeTargets.Class, attributeUsage.ValidOn);
    }
}