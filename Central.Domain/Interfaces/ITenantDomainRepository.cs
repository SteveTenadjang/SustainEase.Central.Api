using Central.Domain.Entities;

namespace Central.Domain.Interfaces;

public interface ITenantDomainRepository : IGenericRepository<TenantDomain>
{
    Task<TenantDomain?> GetByNameAsync(string name);
    Task<List<TenantDomain>> GetByTenantIdAsync(Guid tenantId);
    Task<bool> DomainExistsAsync(string name);
}