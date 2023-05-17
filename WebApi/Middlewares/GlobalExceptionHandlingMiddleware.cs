using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middlewares
{
    public sealed class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly Serilog.ILogger _logger;

        public GlobalExceptionHandlingMiddleware(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.Error("An unhandled exception occurred. Exception: {Exception}", ex);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                ProblemDetails problemDetails = new()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = $"An internal server error has occurred",
                };


                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
