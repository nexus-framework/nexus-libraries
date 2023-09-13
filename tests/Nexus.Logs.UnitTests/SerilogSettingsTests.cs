namespace Nexus.Logs.UnitTests;

[ExcludeFromCodeCoverage]
public class SerilogSettingsTests
{
    [Fact]
    public void SerilogSettings_ElasticSearchSettings_NotNull()
    {
        // Arrange
        var settings = new SerilogSettings();

        // Assert
        Assert.NotNull(settings.ElasticSearchSettings);
    }

    [Fact]
    public void SerilogElasticSearchSettings_Uri_SetAndGet()
    {
        // Arrange
        var uri = "http://localhost:9200";
        var elasticSearchSettings = new SerilogElasticSearchSettings()
        {
            Uri = uri,
            Username = "test",
            Password = "test"
        };

        // Act
        elasticSearchSettings.Uri = uri;
        var result = elasticSearchSettings.Uri;

        // Assert
        Assert.Equal(uri, result);
    }

    [Fact]
    public void SerilogElasticSearchSettings_Username_SetAndGet()
    {
        // Arrange
        var username = "myUsername";
        var elasticSearchSettings = new SerilogElasticSearchSettings()
        {
            Uri = "http://localhost:9200",
            Username = username,
            Password = "test"
        };

        // Act
        elasticSearchSettings.Username = username;
        var result = elasticSearchSettings.Username;

        // Assert
        Assert.Equal(username, result);
    }

    [Fact]
    public void SerilogElasticSearchSettings_Password_SetAndGet()
    {
        // Arrange
        var password = "myPassword";
        var elasticSearchSettings = new SerilogElasticSearchSettings()
        {
            Uri = "http://localhost:9200",
            Username = "test",
            Password = password
        };

        // Act
        elasticSearchSettings.Password = password;
        var result = elasticSearchSettings.Password;

        // Assert
        Assert.Equal(password, result);
    }

    [Fact]
    public void SerilogElasticSearchSettings_IndexFormat_SetAndGet()
    {
        // Arrange
        var elasticSearchSettings = new SerilogElasticSearchSettings()
        {
            Uri = "http://localhost:9200",
            Username = "test",
            Password = "test"
        };
        var indexFormat = "logs-{0:yyyy.MM.dd}";

        // Act
        elasticSearchSettings.IndexFormat = indexFormat;
        var result = elasticSearchSettings.IndexFormat;

        // Assert
        Assert.Equal(indexFormat, result);
    }
    
    [Fact]
    public void SerilogSettings_ElasticSearchSettings_SetAndGet()
    {
        // Arrange
        var serilogSettings = new SerilogSettings();
        serilogSettings.ElasticSearchSettings = new SerilogElasticSearchSettings()
        {
            Uri = "http://localhost:9200",
            Username = "test",
            Password = "test",
            IndexFormat = "logs-{0:yyyy.MM.dd}",
        };
        
        // Assert
        Assert.NotNull(serilogSettings.ElasticSearchSettings);
    }
}