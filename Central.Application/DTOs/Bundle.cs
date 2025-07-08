namespace Central.Application.DTOs;

public record BundleDto(
    Guid Id = default,
    DateTime? CreatedAt = null,
    DateTime? UpdatedAt = null,
    Guid? CreatedBy = null,
    Guid? UpdatedBy = null,
    string Name = "",
    string Key = "",
    string? Description = null
) : BaseDto(Id, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy);

public record CreateBundleRequest(
    string Name,
    string Key,
    string? Description = null
);

public record UpdateBundleRequest(
    Guid Id,
    string Name,
    string Key,
    string? Description = null
);

public record BundleListRequest(
    int Page = 1,
    int PageSize = 10,
    string? SortBy = null,
    bool SortDescending = false,
    string? Search = null,
    string? Name = null,
    string? Key = null
) : PaginatedRequest(Page, PageSize, SortBy, SortDescending, Search);