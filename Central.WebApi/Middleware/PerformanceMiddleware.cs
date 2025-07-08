using System.Diagnostics;

namespace Central.WebApi.Middleware;

public class PerformanceMiddleware(
    RequestDelegate next,
    ILogger<PerformanceMiddleware> logger,
    int warningThresholdMs = 500)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await next(context);

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > warningThresholdMs)
        {
            logger.LogWarning("Slow request detected: {Method} {Path} took {ElapsedMs}ms",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}