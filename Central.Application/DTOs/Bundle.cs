namespace Central.Application.DTOs;

public record BundleDto(
    Guid Id,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    Guid? CreatedBy,
    Guid? UpdatedBy,
    string Name,
    string Key,
    string? Description
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