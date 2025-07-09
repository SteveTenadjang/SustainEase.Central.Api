using Central.Domain.Entities;
using Central.Persistence.Context;

namespace Central.Persistence.Seeders;

public static class BundleSeeder
{
    public static async Task SeedAsync(CentralDbContext dbContext)
    {
        if (dbContext.Bundles.Any()) return;

        var bundles = new List<Bundle>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Carbon Footprint",
                Key = "carbon_footprint",
                Description = "Track and analyze carbon emissions across your organization"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Green IT",
                Key = "green_it",
                Description = "Sustainable IT practices and environmental monitoring"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "RSE & ESG",
                Key = "rse_esg",
                Description = "Corporate social responsibility and ESG reporting tools"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ACV & Eco-conception",
                Key = "acv_eco_conception",
                Description = "Life cycle assessment and eco-design methodologies"
            }
        };

        dbContext.Bundles.AddRange(bundles);
        await dbContext.SaveChangesAsync();
    }
}