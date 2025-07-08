using Central.Domain.Entities;
using Central.Domain.Interfaces;
using Central.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Central.Persistence.Repositories;

public class TenantSubscriptionRepository(CentralDbContext context)
    : GenericRepository<TenantSubscription>(context), ITenantSubscription
{
    public override async Task<TenantSubscription?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .Include(ts => ts.Tenant)
            .Include(ts => ts.Bundle)
            .FirstOrDefaultAsync(ts => ts.Id == id);
    }

    public override async Task<List<TenantSubscription>> GetAllAsync()
    {
        return await DbSet
            .Include(ts => ts.Tenant)
            .Include(ts => ts.Bundle)
            .OrderByDescending(ts => ts.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<TenantSubscription>> GetActiveSubscriptionsAsync()
    {
        return await DbSet
            .Include(ts => ts.Tenant)
            .Include(ts => ts.Bundle)
            .Where(ts => ts.StartDate.AddDays(ts.Duration) >= DateTime.UtcNow)
            .OrderByDescending(ts => ts.CreatedAt)
            .ToListAsync();
    }


    public async Task<List<TenantSubscription>> GetByTenantIdAsync(Guid tenantId)
    {
        return await DbSet
            .Include(ts => ts.Bundle)
            .Where(ts => ts.TenantId == tenantId)
            .OrderByDescending(ts => ts.CreatedAt)
            .ToListAsync();
    }
}