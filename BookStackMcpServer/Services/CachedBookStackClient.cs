using BookStackApiClient;
using BookStackMcpServer.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookStackMcpServer.Services;

public class CachedBookStackClient
{
    private readonly BookStackClient _client;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedBookStackClient> _logger;
    private readonly CachingOptions _cachingOptions;

    public CachedBookStackClient(
        BookStackClient client, 
        IMemoryCache cache, 
        ILogger<CachedBookStackClient> logger,
        IOptions<CachingOptions> cachingOptions)
    {
        _client = client;
        _cache = cache;
        _logger = logger;
        _cachingOptions = cachingOptions.Value;
    }

    private MemoryCacheEntryOptions GetCacheEntryOptions()
    {
        return new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_cachingOptions.AbsoluteExpirationMinutes))
            .SetSlidingExpiration(TimeSpan.FromMinutes(_cachingOptions.SlidingExpirationMinutes));
    }

    public async Task<object> ListBooksAsync(ListingOptions listing)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.ListBooksAsync(listing);
        }

        var cacheKey = $"books_list_{listing.offset}_{listing.count}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await _client.ListBooksAsync(listing);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadBookAsync(int id)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.ReadBookAsync(id);
        }

        var cacheKey = $"book_{id}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedBook) && cachedBook != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedBook;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var book = await _client.ReadBookAsync(id);
        _cache.Set(cacheKey, book, GetCacheEntryOptions());
        return book;
    }

    public async Task<object> ListChaptersAsync(ListingOptions listing)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.ListChaptersAsync(listing);
        }

        var cacheKey = $"chapters_list_{listing.offset}_{listing.count}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await _client.ListChaptersAsync(listing);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadChapterAsync(int id)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.ReadChapterAsync(id);
        }

        var cacheKey = $"chapter_{id}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedChapter) && cachedChapter != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedChapter;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var chapter = await _client.ReadChapterAsync(id);
        _cache.Set(cacheKey, chapter, GetCacheEntryOptions());
        return chapter;
    }

    public async Task<object> ListPagesAsync(ListingOptions listing)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.ListPagesAsync(listing);
        }

        var cacheKey = $"pages_list_{listing.offset}_{listing.count}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await _client.ListPagesAsync(listing);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadPageAsync(int id)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.ReadPageAsync(id);
        }

        var cacheKey = $"page_{id}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedPage) && cachedPage != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedPage;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var page = await _client.ReadPageAsync(id);
        _cache.Set(cacheKey, page, GetCacheEntryOptions());
        return page;
    }

    public async Task<object> ListShelvesAsync(ListingOptions listing)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.ListShelvesAsync(listing);
        }

        var cacheKey = $"shelves_list_{listing.offset}_{listing.count}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await _client.ListShelvesAsync(listing);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadShelfAsync(int id)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.ReadShelfAsync(id);
        }

        var cacheKey = $"shelf_{id}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedShelf) && cachedShelf != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedShelf;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var shelf = await _client.ReadShelfAsync(id);
        _cache.Set(cacheKey, shelf, GetCacheEntryOptions());
        return shelf;
    }

    public async Task<object> ListUsersAsync(ListingOptions listing)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.ListUsersAsync(listing);
        }

        var cacheKey = $"users_list_{listing.offset}_{listing.count}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await _client.ListUsersAsync(listing);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadUserAsync(int id)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.ReadUserAsync(id);
        }

        var cacheKey = $"user_{id}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedUser) && cachedUser != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedUser;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var user = await _client.ReadUserAsync(id);
        _cache.Set(cacheKey, user, GetCacheEntryOptions());
        return user;
    }

    public async Task<object> SearchAsync(SearchArgs args)
    {
        if (!_cachingOptions.Enabled)
        {
            return await _client.SearchAsync(args);
        }

        var cacheKey = $"search_{args.query}_{args.count}_{args.page}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await _client.SearchAsync(args);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }
}
