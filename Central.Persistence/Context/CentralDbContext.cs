using Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Central.Persistence.Context;

public class CentralDbContext(DbContextOptions<CentralDbContext> options) : DbContext(options)
{
    public DbSet<Bundle> Bundles { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantDomain> Domains { get; set; }
    public DbSet<TenantSubscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CentralDbContext).Assembly);

        modelBuilder.Entity<Bundle>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<Tenant>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<TenantDomain>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<TenantSubscription>().HasQueryFilter(e => e.DeletedAt == null);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Add audit fields before saving
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        return await base.SaveChangesAsync(cancellationToken);
    }
}