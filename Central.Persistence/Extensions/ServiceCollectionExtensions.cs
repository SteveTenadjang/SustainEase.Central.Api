using Central.Domain.Interfaces;
using Central.Persistence.Context;
using Central.Persistence.Repositories;
using Central.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Central.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext with PostgreSQL
        services.AddDbContext<CentralDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(CentralDbContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(
                        3,
                        TimeSpan.FromSeconds(30),
                        null);
                });

            // Enable sensitive data logging in development
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development") return;
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        // Register repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IBundleRepository, BundleRepository>();
        services.AddScoped<ITenantDomainRepository, TenantDomainRepository>();
        services.AddScoped<ITenantSubscription, TenantSubscriptionRepository>();

        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}