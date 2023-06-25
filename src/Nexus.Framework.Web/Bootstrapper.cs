using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexus.Configuration;
using Nexus.Logs;
using Serilog;

namespace Nexus.Framework.Web;

/// <summary>
/// Base class for bootstrapping a web application.
/// </summary>
public abstract class Bootstrapper
{
    /// <summary>
    /// The command-line arguments passed to the application.
    /// </summary>
    protected string[] Args;

    /// <summary>
    /// The <see cref="WebApplicationBuilder"/> used to build the web application.
    /// </summary>
    protected readonly WebApplicationBuilder AppBuilder;

    /// <summary>
    /// The built <see cref="WebApplication"/> instance.
    /// </summary>
    protected WebApplication App = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    protected Bootstrapper(string[] args)
    {
        Args = args;
        AppBuilder = WebApplication.CreateBuilder(args);
    }

    /// <summary>
    /// Performs the bootstrap process and runs the web application.
    /// </summary>
    public void BootstrapAndRun()
    {
        try
        {
            AddConfiguration();
            AddLogging();
            AddServices();
            App = AppBuilder.Build();
            ConfigureMiddleware();
            App.Run();
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// Adds configuration to the <see cref="AppBuilder"/>.
    /// </summary>
    protected virtual void AddConfiguration()
    {
        AppBuilder.Configuration.AddCoreConfiguration();
    }

    /// <summary>
    /// Adds logging to the <see cref="AppBuilder"/>.
    /// </summary>
    protected virtual void AddLogging()
    {
        AppBuilder.Logging.AddCoreLogging(AppBuilder.Configuration);
    }

    /// <summary>
    /// Adds services to the <see cref="AppBuilder"/>.
    /// </summary>
    protected virtual void AddServices()
    {
        AppBuilder.Services.AddWebFramework(AppBuilder.Configuration);
    }

    /// <summary>
    /// Configures the middleware pipeline of the web application.
    /// </summary>
    protected virtual void ConfigureMiddleware()
    {
        // This is the default middleware order
        if (App.Environment.IsDevelopment())
        {
            App.UseSwagger();
            App.UseSwaggerUI();
        }

        App.UseHttpsRedirection();
        App.UseCors("AllowAll");
        App.UseAuthentication();
        App.UseAuthorization();

        // TODO: Can probably read config, get frameworksettings, check if telemetry is enabled 
        App.UseOpenTelemetryPrometheusScrapingEndpoint();
    }
}
