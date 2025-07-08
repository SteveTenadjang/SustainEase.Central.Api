using Central.WebApi.Middleware;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace Central.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApiServices(this IServiceCollection services)
    {
        // Add Swagger/OpenAPI
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SustainEase Central API",
                Version = "v1",
                Description = "Central application API for managing tenants, bundles, domains and subscriptions",
                Contact = new OpenApiContact
                {
                    Name = "API Support",
                    Email = "support@central.com"
                }
            });

            // Add examples and descriptions
            c.EnableAnnotations();
        });

        // Add JSON options
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            options.SerializerOptions.WriteIndented = true;
        });
    }

    public static void UseApiMiddleware(this WebApplication app)
    {
        // Add middlewares in correct order
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<PerformanceMiddleware>();

        // Add Swagger in development
        // if (!app.Environment.IsDevelopment()) return;
        app.UseSwagger(opt => opt.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0);
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = "SustainEase Central API";
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "SustainEase Central API v1");
            c.RoutePrefix = string.Empty;
        });
    }
}