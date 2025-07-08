using Central.Domain.Entities;

namespace Central.Domain.Interfaces;

public interface ITenantSubscription : IGenericRepository<TenantSubscription>
{
    Task<List<TenantSubscription>> GetActiveSubscriptionsAsync();
}