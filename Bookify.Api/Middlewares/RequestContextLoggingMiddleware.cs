using Serilog.Context;

namespace Bookify.Api.Middlewares;

public class RequestContextLoggingMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    
    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
        {
            return next(context);
        }
    }
    private object? GetCorrelationId(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);
        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}