using Central.Domain.Entities;
using Central.Domain.Interfaces;
using Central.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Central.Persistence.Repositories;

public class TenantRepository(CentralDbContext context) : GenericRepository<Tenant>(context), ITenantRepository
{
    public override async Task<Tenant?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .Include(t => t.Domains)
            .Include(t => t.Subscriptions)
            .ThenInclude(ts => ts.Bundle)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
        
    public override async Task<List<Tenant>> GetAllAsync()
    {
        return await DbSet
            .Include(t => t.Domains)
            .Include(t => t.Subscriptions)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
        
    public async Task<Tenant?> GetByEmailAsync(string email)
    {
        return await DbSet
            .Include(t => t.Domains)
            .Include(t => t.Subscriptions)
            .FirstOrDefaultAsync(t => t.Email == email);
    }
        
    public async Task<Tenant?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await DbSet
            .Include(t => t.Domains)
            .Include(t => t.Subscriptions)
            .FirstOrDefaultAsync(t => t.PhoneNumber == phoneNumber);
    }
}