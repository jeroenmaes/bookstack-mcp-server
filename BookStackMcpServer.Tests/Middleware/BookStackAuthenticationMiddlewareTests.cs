using BookStackMcpServer.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookStackMcpServer.Tests.Middleware;

public class BookStackAuthenticationMiddlewareTests
{
    private readonly Mock<ILogger<BookStackAuthenticationMiddleware>> _loggerMock;
    
    public BookStackAuthenticationMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<BookStackAuthenticationMiddleware>>();
    }

    [Fact]
    public async Task InvokeAsync_WhenAuthorizationHeaderMissing_Returns401()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new BookStackAuthenticationMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
        Assert.Contains("Missing Authorization header", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_WhenAuthorizationHeaderNotBearer_Returns401()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Headers["Authorization"] = "Basic sometoken";
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new BookStackAuthenticationMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
        Assert.Contains("Invalid Authorization header", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_WhenBearerTokenMissingColon_Returns401()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Headers["Authorization"] = "Bearer invalidtokenformat";
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new BookStackAuthenticationMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
        Assert.Contains("Invalid Bearer token format", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_WhenBearerTokenValid_AllowsRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer validtokenid12345678901234567890:validtokensecret123456789012345678901234567890";
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new BookStackAuthenticationMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextCalled);
        Assert.NotEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        
        // Verify credentials are stored in context
        Assert.True(context.Items.ContainsKey(BookStackAuthenticationMiddleware.BookStackTokenIdContextKey));
        Assert.True(context.Items.ContainsKey(BookStackAuthenticationMiddleware.BookStackTokenSecretContextKey));
        Assert.Equal("validtokenid12345678901234567890", context.Items[BookStackAuthenticationMiddleware.BookStackTokenIdContextKey]);
        Assert.Equal("validtokensecret123456789012345678901234567890", context.Items[BookStackAuthenticationMiddleware.BookStackTokenSecretContextKey]);
    }

    [Fact]
    public async Task InvokeAsync_WhenBearerTokenWithSpecialChars_AllowsRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer token-with-dashes:secret_with_underscores";
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new BookStackAuthenticationMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextCalled);
        Assert.NotEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        
        // Verify credentials are stored correctly
        Assert.Equal("token-with-dashes", context.Items[BookStackAuthenticationMiddleware.BookStackTokenIdContextKey]);
        Assert.Equal("secret_with_underscores", context.Items[BookStackAuthenticationMiddleware.BookStackTokenSecretContextKey]);
    }

    [Fact]
    public async Task InvokeAsync_WhenBearerTokenCaseInsensitive_AllowsRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "bearer validtokenid12345678901234567890:validtokensecret123456789012345678901234567890";
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new BookStackAuthenticationMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextCalled);
        Assert.NotEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenTokenIdEmpty_Returns401()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Headers["Authorization"] = "Bearer :validtokensecret123456789012345678901234567890";
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new BookStackAuthenticationMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
    }
}
