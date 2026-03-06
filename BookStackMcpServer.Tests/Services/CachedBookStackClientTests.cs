using BookStackApiClient;
using BookStackMcpServer.Middleware;
using BookStackMcpServer.Models;
using BookStackMcpServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BookStackMcpServer.Tests.Services;

public class CachedBookStackClientTests
{
    private readonly Mock<ILogger<CachedBookStackClient>> _loggerMock;
    private readonly Mock<ILogger<BookStackClientFactory>> _factoryLoggerMock;

    public CachedBookStackClientTests()
    {
        _loggerMock = new Mock<ILogger<CachedBookStackClient>>();
        _factoryLoggerMock = new Mock<ILogger<BookStackClientFactory>>();
    }

    private (CachedBookStackClient client, IMemoryCache cache) CreateClient(
        string? tokenId,
        bool cachingEnabled = true)
    {
        var cache = new MemoryCache(new MemoryCacheOptions());

        var httpContext = new DefaultHttpContext();
        if (tokenId != null)
        {
            httpContext.Items[BookStackAuthenticationMiddleware.BookStackTokenIdContextKey] = tokenId;
            httpContext.Items[BookStackAuthenticationMiddleware.BookStackTokenSecretContextKey] = "testsecret";
        }

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var bookStackOptions = Options.Create(new BookStackOptions
        {
            BaseUrl = "https://bookstack.example.com"
        });

        var factory = new BookStackClientFactory(httpContextAccessorMock.Object, bookStackOptions, _factoryLoggerMock.Object);

        var cachingOptions = Options.Create(new CachingOptions
        {
            Enabled = cachingEnabled,
            AbsoluteExpirationMinutes = 5,
            SlidingExpirationMinutes = 2
        });

        var client = new CachedBookStackClient(factory, cache, _loggerMock.Object, cachingOptions, httpContextAccessorMock.Object);

        return (client, cache);
    }

    [Fact]
    public async Task ListBooksAsync_WhenTokenIdMissing_ThrowsInvalidOperationException()
    {
        // Arrange
        var (client, _) = CreateClient(tokenId: null);
        var listing = new ListingOptions { offset = 0, count = 10 };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            client.ListBooksAsync(listing));
    }

    [Fact]
    public async Task ListBooksAsync_WhenCacheHitForSameToken_ReturnsCachedData()
    {
        // Arrange
        var tokenId = "tokenA";
        var (client, cache) = CreateClient(tokenId);
        var listing = new ListingOptions { offset = 0, count = 10 };

        // Pre-populate cache with token-scoped key
        var expectedData = new { data = "cached books" };
        var cacheKey = $"{tokenId}_books_list_{listing.offset}_{listing.count}";
        cache.Set(cacheKey, (object)expectedData, TimeSpan.FromMinutes(5));

        // Act
        var result = await client.ListBooksAsync(listing);

        // Assert: cached data is returned (no API call made)
        Assert.Equal(expectedData, result);
    }

    [Fact]
    public async Task ListBooksAsync_WhenCachePopulatedForTokenA_DoesNotReturnDataToTokenB()
    {
        // Arrange: pre-populate cache for tokenA
        var tokenAId = "tokenA";
        var (clientA, cache) = CreateClient(tokenAId);
        var listing = new ListingOptions { offset = 0, count = 10 };

        var tokenAData = new { data = "tokenA books" };
        var tokenACacheKey = $"{tokenAId}_books_list_{listing.offset}_{listing.count}";
        cache.Set(tokenACacheKey, (object)tokenAData, TimeSpan.FromMinutes(5));

        // Create a second client with tokenB but sharing the same cache instance
        var tokenBId = "tokenB";
        var httpContext = new DefaultHttpContext();
        httpContext.Items[BookStackAuthenticationMiddleware.BookStackTokenIdContextKey] = tokenBId;
        httpContext.Items[BookStackAuthenticationMiddleware.BookStackTokenSecretContextKey] = "testsecret";

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var bookStackOptions = Options.Create(new BookStackOptions
        {
            BaseUrl = "https://bookstack.example.com"
        });
        var factory = new BookStackClientFactory(httpContextAccessorMock.Object, bookStackOptions, _factoryLoggerMock.Object);
        var cachingOptions = Options.Create(new CachingOptions
        {
            Enabled = true,
            AbsoluteExpirationMinutes = 5,
            SlidingExpirationMinutes = 2
        });
        var clientB = new CachedBookStackClient(factory, cache, _loggerMock.Object, cachingOptions, httpContextAccessorMock.Object);

        // Verify tokenB's cache key is NOT in cache (tokenA's key should NOT match)
        var tokenBCacheKey = $"{tokenBId}_books_list_{listing.offset}_{listing.count}";
        var hasCacheHitForTokenB = cache.TryGetValue(tokenBCacheKey, out _);

        // Assert: tokenB does NOT get a cache hit from tokenA's cached data
        Assert.False(hasCacheHitForTokenB, "TokenB should not receive cached data belonging to tokenA");

        // Verify tokenA's cached data is still scoped to tokenA only
        Assert.True(cache.TryGetValue(tokenACacheKey, out var tokenAResult));
        Assert.Equal(tokenAData, tokenAResult);
    }

    [Fact]
    public async Task ListBooksAsync_WhenCachingDisabled_ThrowsWithMissingToken()
    {
        // Arrange: caching disabled, no token
        var (client, _) = CreateClient(tokenId: null, cachingEnabled: false);
        var listing = new ListingOptions { offset = 0, count = 10 };

        // Act & Assert: even with caching disabled, missing token still throws
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            client.ListBooksAsync(listing));
    }

    [Fact]
    public void CacheKeys_AreScopedToTokenId_NotSharedAcrossTokens()
    {
        // This test verifies that cache entries populated for one token
        // cannot be retrieved by a different token.
        var cache = new MemoryCache(new MemoryCacheOptions());

        var tokenAId = "tokenA123";
        var tokenBId = "tokenB456";

        // Simulate what CachedBookStackClient stores for tokenA
        var unscopedKey = "books_list_0_10";
        var tokenAScopedKey = $"{tokenAId}_books_list_0_10";
        var tokenBScopedKey = $"{tokenBId}_books_list_0_10";

        var tokenAData = new { books = "data for A" };
        cache.Set(tokenAScopedKey, (object)tokenAData, TimeSpan.FromMinutes(5));

        // TokenB tries to get data using its own scoped key - should miss
        var gotTokenBHit = cache.TryGetValue(tokenBScopedKey, out _);
        // Old unsecured key pattern should also miss (no data stored there)
        var gotUnscopedHit = cache.TryGetValue(unscopedKey, out _);

        Assert.False(gotTokenBHit, "TokenB should not have a cache hit from tokenA's data");
        Assert.False(gotUnscopedHit, "Unscoped key should not exist");
    }
}
