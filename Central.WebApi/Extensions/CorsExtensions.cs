namespace Central.WebApi.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddApiCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            options.AddPolicy("ProductionPolicy", policy =>
            {
                policy.WithOrigins("https://yourdomain.com", "https://app.yourdomain.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static WebApplication UseApiCors(this WebApplication app)
    {
        app.UseCors(app.Environment.IsDevelopment()
            ? "DefaultPolicy"
            : "ProductionPolicy"
        );
        return app;
    }
}