using Central.Application.Common;
using Central.WebApi.Models;

namespace Central.WebApi.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return result.Data != null
                ? Results.Ok(result.Data)
                : Results.NoContent();

        return CreateErrorResponse(result.Error!, result.Errors);
    }

    public static IResult ToHttpResponse(this Result result)
    {
        return result.IsSuccess
            ? Results.NoContent()
            : CreateErrorResponse(result.Error!, result.Errors);
    }

    public static IResult ToCreatedResponse<T>(this Result<T> result, string location)
    {
        return result.IsSuccess
            ? Results.Created(location, result.Data)
            : CreateErrorResponse(result.Error!, result.Errors);
    }

    private static IResult CreateErrorResponse(string error, List<string> errors)
    {
        // Check if it's a validation error (multiple errors)
        if (errors.Count != 0)
        {
            var validationErrors = errors
                .Select((err, index) => new { Key = $"Error{index}", Value = new[] { err } })
                .ToDictionary(x => x.Key, x => x.Value);

            var validationResponse = new ValidationErrorResponse(
                "One or more validation errors occurred.",
                400,
                validationErrors
            );

            return Results.BadRequest(validationResponse);
        }

        // Determine status code based on error message
        var statusCode = DetermineStatusCode(error);
        var errorResponse = new ApiErrorResponse(error, statusCode);

        return statusCode switch
        {
            400 => Results.BadRequest(errorResponse),
            404 => Results.NotFound(errorResponse),
            409 => Results.Conflict(errorResponse),
            _ => Results.Problem(
                title: "An error occurred",
                detail: error,
                statusCode: statusCode
            )
        };
    }

    private static int DetermineStatusCode(string error)
    {
        var lowerError = error.ToLowerInvariant();

        return lowerError switch
        {
            _ when lowerError.Contains("not found") => 404,
            _ when lowerError.Contains("already exists") => 409,
            _ when lowerError.Contains("invalid") || lowerError.Contains("cannot") => 400,
            _ => 500
        };
    }
}