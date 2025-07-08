namespace Central.Application.DTOs;

public record TenantSubscriptionDto(
    Guid Id,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    Guid? CreatedBy,
    Guid? UpdatedBy,
    Guid TenantId,
    Guid BundleId,
    int Duration,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
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