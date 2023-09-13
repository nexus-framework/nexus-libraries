namespace Nexus.Auth.UnitTests;

[ExcludeFromCodeCoverage]
public class ScopeRequirementTests
{
    [Fact]
    public void Constructor_WhenCalled_SetsScope()
    {
        ScopeRequirement sut = new ("scope");

        Assert.Equal("scope", sut.Scope);
    }
}