using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FriendsDebt.Persistence;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var apiDirectory = new[]
            {
                currentDirectory,
                Path.Combine(currentDirectory, "FriendsDebt.Api"),
                Path.Combine(currentDirectory, "..", "FriendsDebt.Api")
            }
            .Select(Path.GetFullPath)
            .FirstOrDefault(path => File.Exists(Path.Combine(path, "appsettings.json")))
            ?? throw new DirectoryNotFoundException("Could not locate the FriendsDebt.Api configuration directory.");

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiDirectory)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Local.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = Environment.GetEnvironmentVariable("FRIENDSDEBT_DB_CONNECTION_STRING")
            ?? configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Database connection string is missing.");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(
                connectionString,
                postgres => postgres.MigrationsHistoryTable("__EFMigrationsHistory", "identity"))
            .Options;

        return new ApplicationDbContext(options);
    }
}
