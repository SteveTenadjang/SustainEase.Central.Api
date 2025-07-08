
using Central.Domain.Entities;
using Central.Domain.Interfaces;
using Central.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Central.Persistence.Repositories;

public class BundleRepository(CentralDbContext context) : GenericRepository<Bundle>(context), IBundleRepository
{
    public override async Task<List<Bundle>> GetAllAsync()
    {
        return await DbSet
            .OrderBy(b => b.Name)
            .ToListAsync();
    }
        
    public async Task<Bundle?> GetByKeyAsync(string key)
    {
        return await DbSet
            .FirstOrDefaultAsync(b => b.Key == key);
    }
        
    public async Task<bool> KeyExistsAsync(string key)
    {
        return await DbSet
            .AnyAsync(b => b.Key == key);
    }
}