namespace Central.Application.DTOs;

public record TenantSubscriptionDto(
    Guid Id = default,
    DateTime? CreatedAt = null,
    DateTime? UpdatedAt = null,
    Guid? CreatedBy = null,
    Guid? UpdatedBy = null,
    Guid TenantId = default,
    Guid BundleId = default,
    int Duration = 0,
    DateTime StartDate = default,
    DateTime EndDate = default,
    bool IsActive = false,
    string? TenantName = null,
    string? BundleName = null
) : BaseDto(Id, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy);

public record CreateTenantSubscriptionRequest(
    Guid TenantId,
    Guid BundleId,
    int Duration,
    DateTime StartDate
);

public record UpdateTenantSubscriptionRequest(
    Guid Id,
    Guid TenantId,
    Guid BundleId,
    int Duration,
    DateTime StartDate
);