using Central.Domain.Entities;

namespace Central.Domain.Interfaces;

public interface ITenantRepository : IGenericRepository<Tenant>
{
    Task<Tenant?> GetByEmailAsync(string email);
    Task<Tenant?> GetByPhoneNumberAsync(string phoneNumber);
}