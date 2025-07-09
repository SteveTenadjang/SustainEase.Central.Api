using Bogus;
using Central.Domain.Entities;
using Central.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Central.Persistence.Seeders.Fakers;

public static class TenantDomainFaker
{
    public static async Task FakerAsync(CentralDbContext dbContext)
    {
        if (dbContext.Domains.Any()) return;

        var tenants = await dbContext.Tenants.ToListAsync();
        var domains = new List<TenantDomain>();

        var domainFaker = new Faker<TenantDomain>()
            .RuleFor(td => td.Id, f => Guid.NewGuid())
            .RuleFor(td => td.Name, f => f.Internet.DomainWord())
            .RuleFor(td => td.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow));

        foreach (var tenant in tenants)
        {
            // Each tenant gets 1-3 domains
            var domainCount = new Random().Next(1, 4);
            for (var i = 0; i < domainCount; i++)
            {
                var domain = domainFaker.Generate();
                domain.TenantId = tenant.Id;
                domain.Name = $"{tenant.Name.ToLower().Replace(" ", "-")}-{i + 1}";
                domains.Add(domain);
            }
        }

        await dbContext.Domains.AddRangeAsync(domains);
        await dbContext.SaveChangesAsync();
    }
}