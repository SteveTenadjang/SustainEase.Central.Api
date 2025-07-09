namespace Central.WebApi.Models;

/// <summary>
///     Standard API response wrapper (for documentation purposes)
/// </summary>
/// <typeparam name="T">The type of data being returned</typeparam>
public record ApiResponse<T>(
    T Data,
    bool Success = true,
    string? Message = null
);