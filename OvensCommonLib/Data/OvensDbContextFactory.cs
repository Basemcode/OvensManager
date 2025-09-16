using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OvensCommonLib.Data;

public class OvensDbContextFactory : IDesignTimeDbContextFactory<OvensDbContext>
{
    public OvensDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // startup project path
            .AddUserSecrets<OvensDbContextFactory>(optional: false) // user secrets
            .Build();

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

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'OvensDatabase' not found.");

        var optionsBuilder = new DbContextOptionsBuilder<OvensDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new OvensDbContext(optionsBuilder.Options);
    }
}
