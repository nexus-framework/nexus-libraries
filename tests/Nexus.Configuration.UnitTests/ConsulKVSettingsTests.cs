namespace Nexus.Configuration.UnitTests;

[ExcludeFromCodeCoverage]
public class ConsulKVSettingsTests
{
    [Fact]
    public void ConsulKVSettings_Url_SetAndGet()
    {
        // Arrange
        ConsulKVSettings settings = new();
        string url = "http://localhost:8500";

        // Act
        settings.Url = url;
        string result = settings.Url;

        // Assert
        Assert.Equal(url, result);
    }

    [Fact]
    public void ConsulKVSettings_Token_SetAndGet()
    {
        // Arrange
        ConsulKVSettings settings = new();
        string token = "myConsulToken";

        // Act
        settings.Token = token;
        string result = settings.Token;

        // Assert
        Assert.Equal(token, result);
    }

    [Fact]
    public void ConsulKVSettings_Key_SetAndGet()
    {
        // Arrange
        ConsulKVSettings settings = new();
        string key = "myKey";

        // Act
        settings.Key = key;
        string result = settings.Key;

        // Assert
        Assert.Equal(key, result);
    }

    [Fact]
    public void ConsulKVSettings_AddGlobalConfig_SetAndGet()
    {
        // Arrange
        ConsulKVSettings settings = new();
        bool addGlobalConfig = true;

        // Act
        settings.AddGlobalConfig = addGlobalConfig;
        bool result = settings.AddGlobalConfig;

        // Assert
        Assert.Equal(addGlobalConfig, result);
    }

    [Fact]
    public void ConsulKVSettings_GlobalConfigKey_SetAndGet()
    {
        // Arrange
        ConsulKVSettings settings = new();
        string globalConfigKey = "globalKey";

        // Act
        settings.GlobalConfigKey = globalConfigKey;
        string result = settings.GlobalConfigKey;

        // Assert
        Assert.Equal(globalConfigKey, result);
    }
}