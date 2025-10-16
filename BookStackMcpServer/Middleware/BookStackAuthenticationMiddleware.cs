using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace BookStackMcpServer.Middleware;

/// <summary>
/// Middleware that extracts BookStack API credentials from request headers and stores them in HttpContext.Items
/// for use by the BookStack API client.
/// </summary>
public partial class BookStackAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<BookStackAuthenticationMiddleware> _logger;

    // BookStack token format validation patterns
    // Token ID: typically alphanumeric, length varies but commonly 32-48 chars
    // Token Secret: typically alphanumeric, length varies but commonly 64-96 chars
    [GeneratedRegex(@"^[a-zA-Z0-9]{10,100}$")]
    private static partial Regex TokenIdPattern();
    
    [GeneratedRegex(@"^[a-zA-Z0-9]{20,200}$")]
    private static partial Regex TokenSecretPattern();

    public const string BookStackTokenIdHeader = "X-BookStack-Token-Id";
    public const string BookStackTokenSecretHeader = "X-BookStack-Token-Secret";
    public const string BookStackTokenIdContextKey = "BookStack.TokenId";
    public const string BookStackTokenSecretContextKey = "BookStack.TokenSecret";

    public BookStackAuthenticationMiddleware(
        RequestDelegate next,
        ILogger<BookStackAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Extract BookStack credentials from headers
        if (!context.Request.Headers.TryGetValue(BookStackTokenIdHeader, out var tokenId))
        {
            _logger.LogWarning("Request missing required header: {HeaderName}", BookStackTokenIdHeader);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Missing BookStack Token ID header");
            return;
        }

        if (!context.Request.Headers.TryGetValue(BookStackTokenSecretHeader, out var tokenSecret))
        {
            _logger.LogWarning("Request missing required header: {HeaderName}", BookStackTokenSecretHeader);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Missing BookStack Token Secret header");
            return;
        }

        var tokenIdValue = tokenId.ToString();
        var tokenSecretValue = tokenSecret.ToString();

        // Validate token format according to BookStack specification
        if (string.IsNullOrWhiteSpace(tokenIdValue) || !TokenIdPattern().IsMatch(tokenIdValue))
        {
            _logger.LogWarning("Invalid BookStack Token ID format");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid BookStack Token ID format");
            return;
        }

        if (string.IsNullOrWhiteSpace(tokenSecretValue) || !TokenSecretPattern().IsMatch(tokenSecretValue))
        {
            _logger.LogWarning("Invalid BookStack Token Secret format");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid BookStack Token Secret format");
            return;
        }

        // Store credentials in HttpContext for use by the BookStack client factory
        context.Items[BookStackTokenIdContextKey] = tokenIdValue;
        context.Items[BookStackTokenSecretContextKey] = tokenSecretValue;

        _logger.LogDebug("BookStack credentials extracted and validated successfully");
        await _next(context);
    }
}
