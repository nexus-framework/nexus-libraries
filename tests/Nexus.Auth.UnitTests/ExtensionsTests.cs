using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Nexus.Auth.UnitTests;

public class ExtensionsTests
{
    [Fact]
    public void AddNexusAuth_RegistersTheExpectedServices()
    {
        // Arrange
        const string resourceName = "TestResource";
        IConfiguration configSubstitute = Substitute.For<IConfiguration>();
        IServiceCollection services = new ServiceCollection();
        
        // Act
        services.AddNexusAuth(configSubstitute, resourceName);

        // Assert
        services.Should().Contain(service => service.ServiceType == typeof(IAuthorizationHandler));
    }
    
    [Fact]
    public void AddNexusAuth_RegistersTheExpectedServices_WhenGivenCustomResourceActions()
    {
        // Arrange
        const string resourceName = "TestResource";
        List<string> actions = new () { "update", "remove" };
        IConfiguration configSubstitute = Substitute.For<IConfiguration>();
        IServiceCollection services = new ServiceCollection();
        
        // Act
        services.AddNexusAuth(configSubstitute, resourceName, actions);

        // Assert
        services.Should().Contain(service => service.ServiceType == typeof(IAuthorizationHandler));
    }
}