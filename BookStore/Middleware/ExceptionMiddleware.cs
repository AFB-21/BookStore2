namespace BookStore.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) { _next = next; _logger = logger; }

        public async Task Invoke(HttpContext httpContext)
        {
            try { await _next(httpContext); }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";
                var response = new { title = "An error occurred", detail = ex.Message };
                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
