namespace Nexus.Common.UnitTests;

[ExcludeFromCodeCoverage]
public class NexusEntityBaseTests
{
    [Fact]
    public void NexusEntityBase_Id_SetAndGet()
    {
        // Arrange
        TestEntity entity = new();
        int id = 42;

        // Act
        entity.Id = id;
        int result = entity.Id;

        // Assert
        Assert.Equal(id, result);
    }
}

// This class inherits from NexusEntityBase for testing purposes.
[ExcludeFromCodeCoverage]
public class TestEntity : NexusEntityBase
{
}