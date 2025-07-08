namespace Central.Application.DTOs;

public record TenantDomainDto(
    Guid Id = default,
    DateTime? CreatedAt = null,
    DateTime? UpdatedAt = null,
    Guid? CreatedBy = null,
    Guid? UpdatedBy = null,
    Guid TenantId = default,
    string Name = "",
    string? TenantName = null
) : BaseDto(Id, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy);

public record CreateTenantDomainRequest(
    Guid TenantId,
    string Name
);

public record UpdateTenantDomainRequest(
    Guid Id,
    Guid TenantId,
    string Name
);