using Microsoft.AspNetCore.Http;
using WebApi.Middlewares;

namespace WebApi.Tests.Middlewares
{
    public sealed class GlobalExceptionHandlingMiddlewareTests
    {
        private readonly GlobalExceptionHandlingMiddleware _middleware;

        public GlobalExceptionHandlingMiddlewareTests()
        {
            _middleware = new();
        }

        [Fact]
        public async Task Invoke_WhenException_ReturnsInternalServerError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var response = context.Response;
            var next = new RequestDelegate(_ => throw new Exception());

            // Act
            await _middleware.InvokeAsync(context, next);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
