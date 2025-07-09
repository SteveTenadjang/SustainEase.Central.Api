using Central.Application.Common;
using Central.Application.DTOs;
using Central.Domain.Entities;

namespace Central.Application.Services.Interfaces;

public interface ITenantSubscriptionService : IGenericService<TenantSubscription, TenantSubscriptionDto,
    CreateTenantSubscriptionRequest, UpdateTenantSubscriptionRequest, PaginatedRequest>
{
    Task<Result<List<TenantSubscriptionDto>>> GetByTenantIdAsync(Guid tenantId);
    Task<Result<List<TenantSubscriptionDto>>> GetActiveSubscriptionsAsync();
    Task<Result<List<TenantSubscriptionDto>>> GetByBundleIdAsync(Guid bundleId);
    Task<Result<bool>> HasActiveSubscriptionAsync(Guid tenantId, Guid bundleId);
    Task<Result<bool>> IsSubscriptionActiveAsync(Guid subscriptionId);
}