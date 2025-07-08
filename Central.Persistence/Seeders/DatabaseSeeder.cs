using Central.Persistence.Context;
using Central.Persistence.Seeders.Fakers;

namespace Central.Persistence.Seeders;

public class DatabaseSeeder(CentralDbContext context)
{
    public async Task SeedAllAsync()
    {
        await context.Database.EnsureCreatedAsync();

        // Run seeders first (static data)
        await BundleSeeder.SeedAsync(context);
    }

    public async Task RunFakersAsync()
    {
        await context.Database.EnsureCreatedAsync();
        
        // Then run fakers (dynamic/test data)
        await TenantDomainFaker.FakerAsync(context);
        await TenantFaker.FakerAsync(context);
        await TenantSubscriptionFaker.FakerAsync(context);
    }
}