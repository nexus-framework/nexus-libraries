using Nexus.Common.Attributes;

namespace Nexus.Common.UnitTests.Attributes;

[ExcludeFromCodeCoverage]
public class NexusMeterAttributeTests
{
    [Fact]
    public void Constructor_SetsNameProperty()
    {
        // Arrange
        string expectedName = "TestMeter";

        // Act
        NexusMeterAttribute attribute = new (expectedName);

        // Assert
        Assert.Equal(expectedName, attribute.Name);
    }
}