namespace Nexus.Auth.UnitTests;

[ExcludeFromCodeCoverage]
public class Auth0SettingsTests
{
    [Fact]
    public void Auth0Settings_Authority_SetAndGet()
    {
        // Arrange
        Auth0Settings auth0Settings = new();
        string authority = "https://my-tenant.auth0.com/";

        // Act
        auth0Settings.Authority = authority;
        string result = auth0Settings.Authority;

        // Assert
        Assert.Equal(authority, result);
    }

    [Fact]
    public void Auth0Settings_Audience_SetAndGet()
    {
        // Arrange
        Auth0Settings auth0Settings = new();
        string audience = "https://api.myapp.com/";

        // Act
        auth0Settings.Audience = audience;
        string result = auth0Settings.Audience;

        // Assert
        Assert.Equal(audience, result);
    }
}