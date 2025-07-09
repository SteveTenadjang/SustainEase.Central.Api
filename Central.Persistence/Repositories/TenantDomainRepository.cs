using Central.Domain.Entities;
using Central.Domain.Interfaces;
using Central.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Central.Persistence.Repositories;

public class TenantDomainRepository(CentralDbContext context)
    : GenericRepository<TenantDomain>(context), ITenantDomainRepository
{
    public override async Task<TenantDomain?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .Include(td => td.Tenant)
            .FirstOrDefaultAsync(td => td.Id == id);
    }

    public async Task<TenantDomain?> GetByNameAsync(string name)
    {
        return await DbSet
            .Include(td => td.Tenant)
            .FirstOrDefaultAsync(td => td.Name == name);
    }

    public async Task<List<TenantDomain>> GetByTenantIdAsync(Guid tenantId)
    {
        return await DbSet
            .Where(td => td.TenantId == tenantId)
            .OrderBy(td => td.Name)
            .ToListAsync();
    }

    public async Task<bool> DomainExistsAsync(string name)
    {
        return await DbSet
            .AnyAsync(td => td.Name == name);
    }
}