using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using OvensCommonLib.Data;
using OvensCommonLib.Services;
using OvensManagerApp.ViewModels;

namespace OvensManagerApp;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Load configuration (including User Secrets)
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<App>()
            .AddEnvironmentVariables();

        IConfiguration configuration = builder.Build();

        var serviceCollection = new ServiceCollection();

        // Register DbContext with connection string from configuration
        // check if we run in development environment
        var environmentName =
            Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

        string? connectionString = string.Empty;
        if (environmentName == "Development")
        {
            connectionString = configuration.GetConnectionString("LocalOvensDatabase");
        }
        else
        {
            connectionString = configuration.GetConnectionString("OvensDatabase");
        }
        /*serviceCollection.AddDbContext<OvensDbContext>(options =>
        {
            options.UseSqlServer(connectionString!);
        });*/

        serviceCollection.AddDbContextFactory<OvensDbContext>(options =>
            options.UseSqlServer(connectionString!)
        );

        // Register OvenLogService service to log the ovens status to the database on the server
        serviceCollection.AddScoped<OvenLogService>();

        // Register OvensDashboardWindowViewModel service to run it from mainWindow
        serviceCollection.AddScoped<OvensDashboardWindowViewModel>();

        Services = serviceCollection.BuildServiceProvider();
    }
}
