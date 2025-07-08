namespace Central.Application.DTOs;

public abstract record BaseDto(
    Guid Id = default,
    DateTime? CreatedAt = null,
    DateTime? UpdatedAt = null,
    Guid? CreatedBy = null,
    Guid? UpdatedBy = null
);

public record PaginatedRequest(
    int Page = 1,
    int PageSize = 10,
    string? SortBy = null,
    bool SortDescending = false,
    string? Search = null
);

public record PaginatedResponse<T>(
    List<T> Items,
    int TotalCount,
    int Page,
    int PageSize
)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}