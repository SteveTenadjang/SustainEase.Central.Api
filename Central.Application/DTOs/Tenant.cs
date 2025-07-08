namespace Central.Application.DTOs;

public record TenantDto(
    Guid Id,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    Guid? CreatedBy,
    Guid? UpdatedBy,
    string Name,
    string Email,
    bool IsActive,
    string? LogoUrl,
    string? PhoneNumber,
    string? PrimaryColor,
    string? SecondaryColor,
    List<TenantDomainDto> Domains,
    List<TenantSubscriptionDto> Subscriptions
) : BaseDto(Id, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy);

public record CreateTenantRequest(
    string Name,
    string Email,
    string? LogoUrl = null,
    string? PhoneNumber = null,
    string? PrimaryColor = null,
    string? SecondaryColor = null,
    List<string> DomainNames = default!
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
