namespace Nexus.Common.UnitTests;

[ExcludeFromCodeCoverage]
public class AuditableNexusEntityBaseTests
{
    [Fact]
    public void AuditableNexusEntityBase_CreatedBy_SetAndGet()
    {
        // Arrange
        TestAuditableEntity entity = new();
        string user = "JohnDoe";

        // Act
        entity.CreatedBy = user;
        string result = entity.CreatedBy;

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public void AuditableNexusEntityBase_CreatedOn_SetAndGet()
    {
        // Arrange
        TestAuditableEntity entity = new();
        DateTime date = DateTime.UtcNow;

        // Act
        entity.CreatedOn = date;
        DateTime result = entity.CreatedOn;

        // Assert
        Assert.Equal(date, result);
    }

    [Fact]
    public void AuditableNexusEntityBase_ModifiedBy_SetAndGet()
    {
        // Arrange
        TestAuditableEntity entity = new();
        string user = "JaneSmith";

        // Act
        entity.ModifiedBy = user;
        string result = entity.ModifiedBy;

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public void AuditableNexusEntityBase_ModifiedOn_SetAndGet()
    {
        // Arrange
        TestAuditableEntity entity = new();
        DateTime date = DateTime.UtcNow;

        // Act
        entity.ModifiedOn = date;
        DateTime result = entity.ModifiedOn;

        // Assert
        Assert.Equal(date, result);
    }
}

// This class inherits from AuditableNexusEntityBase for testing purposes.
[ExcludeFromCodeCoverage]
public class TestAuditableEntity : AuditableNexusEntityBase
{
}
