using Nexus.Common.Abstractions;
using Nexus.Common.Attributes;

namespace Nexus.Common.UnitTests.Attributes;

public class NexusServiceAttributeTest
{
    [Fact]
    public void Constructor_Sets_LifetimeProperty()
    {
        // Arrange
        NexusServiceLifeTime expectedLifetime = NexusServiceLifeTime.Scoped;
        
        // Act
        NexusServiceAttribute<SampleService>? attribute = new NexusServiceAttribute<SampleService>(expectedLifetime);
        
        // Assert
        Assert.Equal(expectedLifetime, attribute.Lifetime);
    }
    
    [Theory]
    [InlineData(NexusServiceLifeTime.Singleton)]
    [InlineData(NexusServiceLifeTime.Scoped)]
    [InlineData(NexusServiceLifeTime.Transient)]
    public void NexusServiceAttribute_Initializes_WithGivenLifetime(NexusServiceLifeTime lifeTime)
    {
        // Arrange and Act
        NexusServiceAttribute<TestNexusService>? attr = new NexusServiceAttribute<TestNexusService>(lifeTime);

        // Assert
        Assert.Equal(lifeTime, attr.Lifetime);
    }

    [Fact]
    public void NexusServiceAttribute_Sets_Lifetime()
    {
        // Arrange
        NexusServiceAttribute<TestNexusService>? attr = new NexusServiceAttribute<TestNexusService>(NexusServiceLifeTime.Singleton);

        // Act and Assert
        Assert.Equal(NexusServiceLifeTime.Singleton, attr.Lifetime);
    }
    
    [Fact]
    public void NexusServiceAttribute_CreatedWithLifetime_SetsLifetimeProperty()
    {
        // Arrange
        NexusServiceLifeTime expectedLifetime = NexusServiceLifeTime.Transient;
        
        // Act
        NexusServiceAttribute? attribute = new NexusServiceAttribute(expectedLifetime);

        // Assert
        Assert.Equal(expectedLifetime, attribute.Lifetime);
    }
}

public class SampleService : INexusService
{
    // Implement required methods and properties for INexusService here
}

public class TestNexusService : INexusService
{
    // Implement required methods and properties for INexusService here
}