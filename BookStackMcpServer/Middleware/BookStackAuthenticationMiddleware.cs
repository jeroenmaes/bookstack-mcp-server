using Microsoft.AspNetCore.Http;

namespace BookStackMcpServer.Middleware;

/// <summary>
/// Middleware that extracts BookStack API credentials from Authorization Bearer token and stores them in HttpContext.Items
/// for use by the BookStack API client.
/// </summary>
public class BookStackAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<BookStackAuthenticationMiddleware> _logger;

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
        
        // Check if token contains a colon separator
        if (!bearerToken.Contains(':'))
        {
            _logger.LogWarning("Invalid Bearer token format. Token must contain a colon separator");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid Bearer token format. Expected format: Bearer <token_id>:<token_secret>");
            return;
        }

        var parts = bearerToken.Split(':', 2);
        var tokenIdValue = parts[0];
        var tokenSecretValue = parts[1];

        // Basic validation - ensure neither part is empty
        if (string.IsNullOrWhiteSpace(tokenIdValue) || string.IsNullOrWhiteSpace(tokenSecretValue))
        {
            _logger.LogWarning("Invalid Bearer token. Token ID and Secret cannot be empty");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Token ID and Secret cannot be empty");
            return;
        }

        // Store credentials in HttpContext for use by the BookStack client factory
        context.Items[BookStackTokenIdContextKey] = tokenIdValue;
        context.Items[BookStackTokenSecretContextKey] = tokenSecretValue;

        _logger.LogDebug("BookStack credentials extracted successfully from Bearer token");
        await _next(context);
    }
}
