using BookStackApiClient;
using BookStackMcpServer.Middleware;
using BookStackMcpServer.Models;
using Microsoft.Extensions.Options;

namespace BookStackMcpServer.Services;

/// <summary>
/// Factory service that creates BookStackClient instances using credentials from the current HTTP request context.
/// </summary>
public class BookStackClientFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<BookStackOptions> _bookStackOptions;
    private readonly ILogger<BookStackClientFactory> _logger;

    public BookStackClientFactory(
        IHttpContextAccessor httpContextAccessor,
        IOptions<BookStackOptions> bookStackOptions,
        ILogger<BookStackClientFactory> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _bookStackOptions = bookStackOptions;
        _logger = logger;
    }

    /// <summary>
    /// Creates a BookStackClient instance using credentials from the current HTTP request.
    /// </summary>
    /// <returns>A configured BookStackClient instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when credentials are not found in the request context.</exception>
    public BookStackClient CreateClient()
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext is not available");

        // Get credentials from HttpContext.Items (set by BookStackAuthenticationMiddleware)
        if (!httpContext.Items.TryGetValue(BookStackAuthenticationMiddleware.BookStackTokenIdContextKey, out var tokenIdObj) ||
            tokenIdObj is not string tokenId)
        {
            _logger.LogError("BookStack Token ID not found in HttpContext");
            throw new InvalidOperationException("BookStack credentials not found in request context");
        }

        if (!httpContext.Items.TryGetValue(BookStackAuthenticationMiddleware.BookStackTokenSecretContextKey, out var tokenSecretObj) ||
            tokenSecretObj is not string tokenSecret)
        {
            _logger.LogError("BookStack Token Secret not found in HttpContext");
            throw new InvalidOperationException("BookStack credentials not found in request context");
        }

        var bookStackOptions = _bookStackOptions.Value;
        if (string.IsNullOrWhiteSpace(bookStackOptions.BaseUrl))
        {
            _logger.LogError("BookStack BaseUrl not configured");
            throw new InvalidOperationException("BookStack BaseUrl is not configured");
        }

        // Create BookStackClient with per-request credentials
        var baseUrl = bookStackOptions.BaseUrl.TrimEnd('/');
        var apiUri = new Uri($"{baseUrl}/api/");
        
        _logger.LogDebug("Creating BookStackClient for request with BaseUrl: {BaseUrl}", baseUrl);
        return new BookStackClient(apiUri, tokenId, tokenSecret);
    }
}
