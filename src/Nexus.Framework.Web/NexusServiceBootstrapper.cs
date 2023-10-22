using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexus.Framework.Web.Configuration;
using Nexus.Framework.Web.Logging;
using Nexus.Persistence;
using Serilog;

namespace Nexus.Framework.Web;

/// <summary>
/// Base class for bootstrapping a web application.
/// </summary>
public abstract class NexusServiceBootstrapper
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
    /// Initializes a new instance of the <see cref="NexusServiceBootstrapper"/> class.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    protected NexusServiceBootstrapper(string[] args)
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
        AppBuilder.Configuration.ConfigureWebFramework();
    }

    /// <summary>
    /// Adds logging to the <see cref="AppBuilder"/>.
    /// </summary>
    protected virtual void AddLogging()
    {
        AppBuilder.Logging.ConfigureWebFrameworkLogging(AppBuilder.Configuration);
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
        FrameworkSettings settings = new ();
        App.Configuration.GetRequiredSection(nameof(FrameworkSettings)).Bind(settings);

        // This is the default middleware order
        if (App.Environment.IsDevelopment() && settings.Swagger is { Enable: true })
        {
            App.UseSwagger();
            App.UseSwaggerUI();
        }

        App.UseHttpsRedirection();
        App.UseCors("AllowAll");

        if (settings.Auth is { Enable: true })
        {
            App.UseAuthentication();
            App.UseAuthorization();
        }

        if (settings.Telemetry is { Enable: true })
        {
            App.UseOpenTelemetryPrometheusScrapingEndpoint();
        }

        ApplyMigrations();
    }

    private void ApplyMigrations()
    {
        List<Type> dbContextTypes = Assembly.GetEntryAssembly()!.GetTypes().Where(t => t.IsSubclassOf(typeof(AuditableDbContext))).ToList();
        if (dbContextTypes.Count == 0)
        {
            return;
        }
        
        using IServiceScope scope = App.Services.CreateScope();
        foreach (Type dbContextType in dbContextTypes)
        {
            DbContext? context = scope.ServiceProvider.GetRequiredService(dbContextType) as DbContext;
            context?.Database.Migrate();
        }
    }
}
