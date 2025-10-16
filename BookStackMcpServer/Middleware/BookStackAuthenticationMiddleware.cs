using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace BookStackMcpServer.Middleware;

/// <summary>
/// Middleware that extracts BookStack API credentials from Authorization Bearer token and stores them in HttpContext.Items
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
        // Extract credentials from Authorization Bearer token
        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            _logger.LogWarning("Request missing Authorization header");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Missing Authorization header. Expected format: Authorization: Bearer <token_id>:<token_secret>");
            return;
        }

        var authHeaderValue = authHeader.ToString();
        if (!authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Invalid Authorization header format. Expected Bearer token");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid Authorization header. Expected format: Authorization: Bearer <token_id>:<token_secret>");
            return;
        }

        var bearerToken = authHeaderValue.Substring(7); // Remove "Bearer " prefix
        var parts = bearerToken.Split(':', 2);
        
        if (parts.Length != 2)
        {
            _logger.LogWarning("Invalid Bearer token format. Expected format: Bearer <token_id>:<token_secret>");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid Bearer token format. Expected format: Bearer <token_id>:<token_secret>");
            return;
        }

        var tokenIdValue = parts[0];
        var tokenSecretValue = parts[1];

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

        _logger.LogDebug("BookStack credentials extracted and validated successfully from Bearer token");
        await _next(context);
    }
}
