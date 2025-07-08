namespace Central.Application.DTOs;

public record TenantDomainDto(
    Guid Id,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    Guid? CreatedBy,
    Guid? UpdatedBy,
    Guid TenantId,
    string Name,
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