using Central.Domain.Entities;

namespace Central.Domain.Interfaces;

public interface IBundleRepository : IGenericRepository<Bundle>
{
    Task<Bundle?> GetByKeyAsync(string key);
    Task<bool> KeyExistsAsync(string key);
}