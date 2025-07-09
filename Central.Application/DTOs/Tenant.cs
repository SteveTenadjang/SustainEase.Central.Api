namespace Central.Application.DTOs;

public record TenantDto(
    Guid Id = default,
    DateTime? CreatedAt = null,
    DateTime? UpdatedAt = null,
    Guid? CreatedBy = null,
    Guid? UpdatedBy = null,
    string Name = "",
    string Email = "",
    bool IsActive = false,
    string? LogoUrl = null,
    string? PhoneNumber = null,
    string? PrimaryColor = null,
    string? SecondaryColor = null,
    List<TenantDomainDto>? Domains = null,
    List<TenantSubscriptionDto>? Subscriptions = null
) : BaseDto(Id, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy)
{
    public List<TenantDomainDto> Domains { get; init; } = Domains ?? [];
    public List<TenantSubscriptionDto> Subscriptions { get; init; } = Subscriptions ?? [];
}

public record CreateTenantRequest(
    string Name,
    string Email,
    string? LogoUrl = null,
    string? PhoneNumber = null,
    string? PrimaryColor = null,
    string? SecondaryColor = null,
    List<string> DomainNames = null!
)
{
    public List<string> DomainNames { get; init; } = DomainNames ?? new List<string>();
}

public record UpdateTenantRequest(
    Guid Id,
    string Name,
    string Email,
    bool IsActive,
    string? LogoUrl = null,
    string? PhoneNumber = null,
    string? PrimaryColor = null,
    string? SecondaryColor = null
);

public record TenantListRequest(
    int Page = 1,
    int PageSize = 10,
    string? SortBy = null,
    bool SortDescending = false,
    string? Search = null,
    string? Name = null,
    string? Email = null,
    bool? IsActive = null
) : PaginatedRequest(Page, PageSize, SortBy, SortDescending, Search);