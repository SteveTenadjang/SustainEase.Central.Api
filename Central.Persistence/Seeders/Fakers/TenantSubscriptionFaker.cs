using Bogus;
using Central.Domain.Entities;
using Central.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Central.Persistence.Seeders.Fakers;

public static class TenantSubscriptionFaker
{
    public static async Task FakerAsync(CentralDbContext dbContext)
    {
        if (dbContext.Subscriptions.Any()) return;

        var tenants = await dbContext.Tenants.ToListAsync();
        var bundles = await dbContext.Bundles.ToListAsync();
        var subscriptions = new List<TenantSubscription>();

        var subscriptionFaker = new Faker<TenantSubscription>()
            .RuleFor(ts => ts.Id, f => Guid.NewGuid())
            .RuleFor(ts => ts.Duration, f => f.PickRandom(30, 90, 180, 365))
            .RuleFor(ts => ts.StartDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(-6), DateTime.UtcNow.AddMonths(1)))
            .RuleFor(ts => ts.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(-6), DateTime.UtcNow));

        foreach (var tenant in tenants)
        {
            // Each tenant gets 1-2 random bundle subscriptions
            var bundleCount = new Random().Next(1, 3);
            var selectedBundles = bundles.OrderBy(x => Guid.NewGuid()).Take(bundleCount);

            foreach (var bundle in selectedBundles)
            {
                var subscription = subscriptionFaker.Generate();
                subscription.TenantId = tenant.Id;
                subscription.BundleId = bundle.Id;
                subscriptions.Add(subscription);
            }
        }

        await dbContext.Subscriptions.AddRangeAsync(subscriptions);
        await dbContext.SaveChangesAsync();
    }
}