namespace Central.WebApi.Models;

public record ApiErrorResponse(
    string Message,
    int StatusCode,
    string? Detail = null,
    Dictionary<string, string[]>? ValidationErrors = null
);

public record ValidationErrorResponse(
    string Message,
    int StatusCode,
    Dictionary<string, string[]> ValidationErrors
) : ApiErrorResponse(Message, StatusCode, null, ValidationErrors);