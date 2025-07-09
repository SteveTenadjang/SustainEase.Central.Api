using Central.WebApi.Models;
using FluentValidation;

namespace Central.WebApi.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var model = context.Arguments.OfType<T>().FirstOrDefault();

        if (model == null)
            return await next(context);

        var validationResult = await validator.ValidateAsync(model);

        if (validationResult.IsValid) return await next(context);

        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(e => e.ErrorMessage).ToArray()
            );

        var response = new ValidationErrorResponse(
            "One or more validation errors occurred.",
            400,
            errors
        );

        return Results.BadRequest(response);
    }
}