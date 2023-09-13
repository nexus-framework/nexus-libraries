namespace Nexus.Logs.UnitTests;

[ExcludeFromCodeCoverage]
public class SerilogSettingsTests
{
    [Fact]
    public void SerilogSettings_ElasticSearchSettings_NotNull()
    {
        // Arrange
        SerilogSettings settings = new();

        // Assert
        Assert.NotNull(settings.ElasticSearchSettings);
    }

    [Fact]
    public void SerilogElasticSearchSettings_Uri_SetAndGet()
    {
        // Arrange
        string uri = "http://localhost:9200";
        SerilogElasticSearchSettings elasticSearchSettings = new()
        {
            Uri = uri,
            Username = "test",
            Password = "test"
        };

        // Act
        elasticSearchSettings.Uri = uri;
        string result = elasticSearchSettings.Uri;

        // Assert
        Assert.Equal(uri, result);
    }

    [Fact]
    public void SerilogElasticSearchSettings_Username_SetAndGet()
    {
        // Arrange
        string username = "myUsername";
        SerilogElasticSearchSettings elasticSearchSettings = new()
        {
            Uri = "http://localhost:9200",
            Username = username,
            Password = "test"
        };

        // Act
        elasticSearchSettings.Username = username;
        string result = elasticSearchSettings.Username;

        // Assert
        Assert.Equal(username, result);
    }

    [Fact]
    public void SerilogElasticSearchSettings_Password_SetAndGet()
    {
        // Arrange
        string password = "myPassword";
        SerilogElasticSearchSettings elasticSearchSettings = new()
        {
            Uri = "http://localhost:9200",
            Username = "test",
            Password = password
        };

        // Act
        elasticSearchSettings.Password = password;
        string result = elasticSearchSettings.Password;

        // Assert
        Assert.Equal(password, result);
    }

    [Fact]
    public void SerilogElasticSearchSettings_IndexFormat_SetAndGet()
    {
        // Arrange
        SerilogElasticSearchSettings elasticSearchSettings = new()
        {
            Uri = "http://localhost:9200",
            Username = "test",
            Password = "test"
        };
        string indexFormat = "logs-{0:yyyy.MM.dd}";

        // Act
        elasticSearchSettings.IndexFormat = indexFormat;
        string result = elasticSearchSettings.IndexFormat;

        // Assert
        Assert.Equal(indexFormat, result);
    }
    
    [Fact]
    public void SerilogSettings_ElasticSearchSettings_SetAndGet()
    {
        // Arrange
        SerilogSettings serilogSettings = new();
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