namespace EgeTutor.API.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.LogInformation("Starting request: {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                await _next(context);

                _logger.LogInformation("Completed request: {Method} {Path} - Status: {StatusCode}",
                    context.Request.Method, context.Request.Path, context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in request: {Method} {Path}",
                    context.Request.Method, context.Request.Path);
                throw;
            }
        }
    }
}
