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
        string tokenIdValue;
        string tokenSecretValue;

        // Try to extract credentials from Authorization Bearer token first
        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var authHeaderValue = authHeader.ToString();
            if (authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var bearerToken = authHeaderValue.Substring(7); // Remove "Bearer " prefix
                var parts = bearerToken.Split(':', 2);
                
                if (parts.Length == 2)
                {
                    tokenIdValue = parts[0];
                    tokenSecretValue = parts[1];
                    _logger.LogDebug("BookStack credentials extracted from Authorization Bearer token");
                }
                else
                {
                    _logger.LogWarning("Invalid Bearer token format. Expected format: Bearer <token_id>:<token_secret>");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid Bearer token format. Expected format: Bearer <token_id>:<token_secret>");
                    return;
                }
            }
            else
            {
                // Authorization header exists but is not Bearer type, fall through to check individual headers
                tokenIdValue = null!;
                tokenSecretValue = null!;
            }
        }
        else
        {
            tokenIdValue = null!;
            tokenSecretValue = null!;
        }

        // If not found in Authorization header, try individual headers
        if (string.IsNullOrEmpty(tokenIdValue) || string.IsNullOrEmpty(tokenSecretValue))
        {
            if (!context.Request.Headers.TryGetValue(BookStackTokenIdHeader, out var tokenId))
            {
                _logger.LogWarning("Request missing required authentication. Provide either Authorization Bearer token or X-BookStack-Token-Id/X-BookStack-Token-Secret headers");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Missing BookStack authentication. Provide either Authorization Bearer token or X-BookStack-Token-Id/X-BookStack-Token-Secret headers");
                return;
            }

            if (!context.Request.Headers.TryGetValue(BookStackTokenSecretHeader, out var tokenSecret))
            {
                _logger.LogWarning("Request missing required header: {HeaderName}", BookStackTokenSecretHeader);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Missing BookStack Token Secret header");
                return;
            }

            tokenIdValue = tokenId.ToString();
            tokenSecretValue = tokenSecret.ToString();
            _logger.LogDebug("BookStack credentials extracted from individual headers");
        }

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
