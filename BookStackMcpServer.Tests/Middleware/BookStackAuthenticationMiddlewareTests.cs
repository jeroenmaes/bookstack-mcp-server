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
    public async Task InvokeAsync_WhenTokenIdMissing_Returns401()
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
        Assert.Contains("Missing BookStack Token ID header", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_WhenTokenSecretMissing_Returns401()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Headers[BookStackAuthenticationMiddleware.BookStackTokenIdHeader] = "validtokenid12345678901234567890";
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
        Assert.Contains("Missing BookStack Token Secret header", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_WhenTokenIdInvalid_Returns401()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Headers[BookStackAuthenticationMiddleware.BookStackTokenIdHeader] = "invalid!token";
        context.Request.Headers[BookStackAuthenticationMiddleware.BookStackTokenSecretHeader] = "valid-token-secret-123456789012345678901234567890";
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
        Assert.Contains("Invalid BookStack Token ID format", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_WhenTokenSecretInvalid_Returns401()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Headers[BookStackAuthenticationMiddleware.BookStackTokenIdHeader] = "validtokenid12345678901234567890";
        context.Request.Headers[BookStackAuthenticationMiddleware.BookStackTokenSecretHeader] = "short";
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
        Assert.Contains("Invalid BookStack Token Secret format", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_WhenTokensValid_AllowsRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers[BookStackAuthenticationMiddleware.BookStackTokenIdHeader] = "validtokenid12345678901234567890";
        context.Request.Headers[BookStackAuthenticationMiddleware.BookStackTokenSecretHeader] = "validtokensecret123456789012345678901234567890";
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
    public async Task InvokeAsync_WhenTokenIdEmpty_Returns401()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Headers[BookStackAuthenticationMiddleware.BookStackTokenIdHeader] = "";
        context.Request.Headers[BookStackAuthenticationMiddleware.BookStackTokenSecretHeader] = "validtokensecret123456789012345678901234567890";
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
