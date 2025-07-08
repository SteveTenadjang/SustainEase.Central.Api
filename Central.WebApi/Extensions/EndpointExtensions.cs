using Central.WebApi.Endpoints;

namespace Central.WebApi.Extensions;

public static class EndpointExtensions
{
    public static void MapApiEndpoints(this WebApplication app)
    {
        var apiGroup = app.MapGroup("/api")
            .WithOpenApi();

        // Map all entity endpoints
        apiGroup.MapBundleEndpoints();
        apiGroup.MapTenantEndpoints();
        apiGroup.MapTenantDomainEndpoints();
        apiGroup.MapTenantSubscriptionEndpoints();

        // Add health check endpoint
        app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
            .WithName("HealthCheck")
            .WithTags("Health")
            .WithSummary("Health check endpoint")
            .Produces<object>(200);
    }
}