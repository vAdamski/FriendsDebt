using FriendsDebt.Application.Common.Interfaces.Persistence;
using FriendsDebt.Domain.UserAccounts;
using FriendsDebt.Persistence.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FriendsDebt.Persistence;

public static class DependencyInjection
{
    private const string ConnectionStringEnvironmentVariable = "FRIENDSDEBT_DB_CONNECTION_STRING";

    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariable)
            ?? configuration.GetConnectionString("Database");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Database connection string is missing. Configure ConnectionStrings:Database or {ConnectionStringEnvironmentVariable}.");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                postgres =>
                {
                    postgres.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    postgres.MigrationsHistoryTable("__EFMigrationsHistory", "identity");
                    postgres.EnableRetryOnFailure();
                }));

        services.TryAddSingleton(TimeProvider.System);
        services.AddScoped<IApplicationDbContext>(serviceProvider =>
            serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.AddDataProtection();

        services
            .AddIdentityApiEndpoints<UserAccount>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddClaimsPrincipalFactory<UserAccountClaimsPrincipalFactory>();

        return services;
    }
}
