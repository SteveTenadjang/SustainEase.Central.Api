using Bogus;
using Central.Domain.Entities;
using Central.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Central.Persistence.Seeders.Fakers;

public static class TenantFaker
{
    public static async Task FakerAsync(CentralDbContext dbContext)
    {
        if (await dbContext.Tenants.AnyAsync()) return;

        var tenantFaker = new Faker<Tenant>()
            .RuleFor(t => t.Id, f => Guid.NewGuid())
            .RuleFor(t => t.Name, f => f.Company.CompanyName())
            .RuleFor(t => t.Email, f => f.Internet.Email())
            .RuleFor(t => t.IsActive, f => f.Random.Bool(0.9f)) // 90% active
            .RuleFor(t => t.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(t => t.PrimaryColor, f => f.Internet.Color())
            .RuleFor(t => t.SecondaryColor, f => f.Internet.Color())
            .RuleFor(t => t.LogoUrl, f => f.Internet.Avatar())
            .RuleFor(t => t.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddYears(-2), DateTime.UtcNow));

        var tenants = tenantFaker.Generate(50);
            
        await dbContext.Tenants.AddRangeAsync(tenants);
        await dbContext.SaveChangesAsync();
    }
}