using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middlewares
{
    public sealed class GlobalExceptionHandlingMiddleware : IMiddleware
    {

        public GlobalExceptionHandlingMiddleware() { }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception)
            {

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                ProblemDetails problemDetails = new()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = "An internal server error has occurred",
                };


                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
