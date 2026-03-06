using BookStackApiClient;
using BookStackMcpServer.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookStackMcpServer.Services;

public class CachedBookStackClient
{
    private readonly BookStackClientFactory _clientFactory;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedBookStackClient> _logger;
    private readonly CachingOptions _cachingOptions;

    public CachedBookStackClient(
        BookStackClientFactory clientFactory, 
        IMemoryCache cache, 
        ILogger<CachedBookStackClient> logger,
        IOptions<CachingOptions> cachingOptions)
    {
        _clientFactory = clientFactory;
        _cache = cache;
        _logger = logger;
        _cachingOptions = cachingOptions.Value;
    }

    private BookStackClient GetClient()
    {
        return _clientFactory.CreateClient();
    }

    private MemoryCacheEntryOptions GetCacheEntryOptions()
    {
        return new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_cachingOptions.AbsoluteExpirationMinutes))
            .SetSlidingExpiration(TimeSpan.FromMinutes(_cachingOptions.SlidingExpirationMinutes));
    }

    public async Task<object> ListBooksAsync(ListingOptions listing, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.ListBooksAsync(listing, cancellationToken);
        }

        var cacheKey = $"books_list_{listing.offset}_{listing.count}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.ListBooksAsync(listing, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadBookAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.ReadBookAsync(id, cancellationToken);
        }

        var cacheKey = $"book_{id}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedBook) && cachedBook != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedBook;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var book = await client.ReadBookAsync(id, cancellationToken);
        _cache.Set(cacheKey, book, GetCacheEntryOptions());
        return book;
    }

    public async Task<object> ListChaptersAsync(ListingOptions listing, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.ListChaptersAsync(listing, cancellationToken);
        }

        var cacheKey = $"chapters_list_{listing.offset}_{listing.count}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.ListChaptersAsync(listing, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadChapterAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.ReadChapterAsync(id, cancellationToken);
        }

        var cacheKey = $"chapter_{id}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedChapter) && cachedChapter != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedChapter;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var chapter = await client.ReadChapterAsync(id, cancellationToken);
        _cache.Set(cacheKey, chapter, GetCacheEntryOptions());
        return chapter;
    }

    public async Task<object> ListPagesAsync(ListingOptions listing, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.ListPagesAsync(listing, cancellationToken);
        }

        var cacheKey = $"pages_list_{listing.offset}_{listing.count}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.ListPagesAsync(listing, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadPageAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.ReadPageAsync(id, cancellationToken);
        }

        var cacheKey = $"page_{id}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedPage) && cachedPage != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedPage;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var page = await client.ReadPageAsync(id, cancellationToken);
        _cache.Set(cacheKey, page, GetCacheEntryOptions());
        return page;
    }

    public async Task<object> ListShelvesAsync(ListingOptions listing, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.ListShelvesAsync(listing, cancellationToken);
        }

        var cacheKey = $"shelves_list_{listing.offset}_{listing.count}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.ListShelvesAsync(listing, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadShelfAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.ReadShelfAsync(id, cancellationToken);
        }

        var cacheKey = $"shelf_{id}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedShelf) && cachedShelf != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedShelf;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var shelf = await client.ReadShelfAsync(id, cancellationToken);
        _cache.Set(cacheKey, shelf, GetCacheEntryOptions());
        return shelf;
    }

    public async Task<object> ListUsersAsync(ListingOptions listing, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.ListUsersAsync(listing, cancellationToken);
        }

        var cacheKey = $"users_list_{listing.offset}_{listing.count}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.ListUsersAsync(listing, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadUserAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.ReadUserAsync(id, cancellationToken);
        }

        var cacheKey = $"user_{id}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedUser) && cachedUser != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedUser;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var user = await client.ReadUserAsync(id, cancellationToken);
        _cache.Set(cacheKey, user, GetCacheEntryOptions());
        return user;
    }

    public async Task<object> SearchAsync(SearchArgs args, CancellationToken cancellationToken = default)
    {
        var client = GetClient();
        
        if (!_cachingOptions.Enabled)
        {
            return await client.SearchAsync(args, cancellationToken);
        }

        var cacheKey = $"search_{args.query}_{args.count}_{args.page}";
        
        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.SearchAsync(args, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ListRolesAsync(ListingOptions listing, CancellationToken cancellationToken = default)
    {
        var client = GetClient();

        if (!_cachingOptions.Enabled)
        {
            return await client.ListRolesAsync(listing, cancellationToken);
        }

        var cacheKey = $"roles_list_{listing.offset}_{listing.count}";

        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.ListRolesAsync(listing, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadRoleAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = GetClient();

        if (!_cachingOptions.Enabled)
        {
            return await client.ReadRoleAsync(id, cancellationToken);
        }

        var cacheKey = $"role_{id}";

        if (_cache.TryGetValue(cacheKey, out object? cachedRole) && cachedRole != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedRole;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var role = await client.ReadRoleAsync(id, cancellationToken);
        _cache.Set(cacheKey, role, GetCacheEntryOptions());
        return role;
    }

    public async Task<object> ListAttachmentsAsync(ListingOptions listing, CancellationToken cancellationToken = default)
    {
        var client = GetClient();

        if (!_cachingOptions.Enabled)
        {
            return await client.ListAttachmentsAsync(listing, cancellationToken);
        }

        var cacheKey = $"attachments_list_{listing.offset}_{listing.count}";

        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.ListAttachmentsAsync(listing, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadAttachmentAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = GetClient();

        if (!_cachingOptions.Enabled)
        {
            return await client.ReadAttachmentAsync(id, cancellationToken);
        }

        var cacheKey = $"attachment_{id}";

        if (_cache.TryGetValue(cacheKey, out object? cachedAttachment) && cachedAttachment != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedAttachment;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var attachment = await client.ReadAttachmentAsync(id, cancellationToken);
        _cache.Set(cacheKey, attachment, GetCacheEntryOptions());
        return attachment;
    }

    public async Task<object> ListRecycleBinAsync(ListingOptions listing, CancellationToken cancellationToken = default)
    {
        var client = GetClient();

        if (!_cachingOptions.Enabled)
        {
            return await client.ListRecycleBinAsync(listing, cancellationToken);
        }

        var cacheKey = $"recyclebin_list_{listing.offset}_{listing.count}";

        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.ListRecycleBinAsync(listing, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ListAuditLogAsync(ListingOptions listing, CancellationToken cancellationToken = default)
    {
        var client = GetClient();

        if (!_cachingOptions.Enabled)
        {
            return await client.ListAuditLogAsync(listing, cancellationToken);
        }

        var cacheKey = $"auditlog_list_{listing.offset}_{listing.count}";

        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.ListAuditLogAsync(listing, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadContentPermissionsAsync(string contentType, int id, CancellationToken cancellationToken = default)
    {
        var client = GetClient();

        if (!_cachingOptions.Enabled)
        {
            return await client.ReadContentPermissionsAsync(contentType, id, cancellationToken);
        }

        var cacheKey = $"permissions_{contentType}_{id}";

        if (_cache.TryGetValue(cacheKey, out object? cachedPermissions) && cachedPermissions != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedPermissions;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var permissions = await client.ReadContentPermissionsAsync(contentType, id, cancellationToken);
        _cache.Set(cacheKey, permissions, GetCacheEntryOptions());
        return permissions;
    }

    public async Task<string> ExportBookHtmlAsync(int id, CancellationToken cancellationToken = default)
        => await GetClient().ExportBookHtmlAsync(id, cancellationToken);

    public async Task<string> ExportBookMarkdownAsync(int id, CancellationToken cancellationToken = default)
        => await GetClient().ExportBookMarkdownAsync(id, cancellationToken);

    public async Task<string> ExportBookPlainAsync(int id, CancellationToken cancellationToken = default)
        => await GetClient().ExportBookPlainAsync(id, cancellationToken);

    public async Task<string> ExportChapterHtmlAsync(int id, CancellationToken cancellationToken = default)
        => await GetClient().ExportChapterHtmlAsync(id, cancellationToken);

    public async Task<string> ExportChapterMarkdownAsync(int id, CancellationToken cancellationToken = default)
        => await GetClient().ExportChapterMarkdownAsync(id, cancellationToken);

    public async Task<string> ExportChapterPlainAsync(int id, CancellationToken cancellationToken = default)
        => await GetClient().ExportChapterPlainAsync(id, cancellationToken);

    public async Task<string> ExportPageHtmlAsync(int id, CancellationToken cancellationToken = default)
        => await GetClient().ExportPageHtmlAsync(id, cancellationToken);

    public async Task<string> ExportPageMarkdownAsync(int id, CancellationToken cancellationToken = default)
        => await GetClient().ExportPageMarkdownAsync(id, cancellationToken);

    public async Task<string> ExportPagePlainAsync(int id, CancellationToken cancellationToken = default)
        => await GetClient().ExportPagePlainAsync(id, cancellationToken);

    public async Task<object> ListImagesAsync(ListingOptions listing, CancellationToken cancellationToken = default)
    {
        var client = GetClient();

        if (!_cachingOptions.Enabled)
        {
            return await client.ListImagesAsync(listing, cancellationToken);
        }

        var cacheKey = $"images_list_{listing.offset}_{listing.count}";

        if (_cache.TryGetValue(cacheKey, out object? cachedResponse) && cachedResponse != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var response = await client.ListImagesAsync(listing, cancellationToken);
        _cache.Set(cacheKey, response, GetCacheEntryOptions());
        return response;
    }

    public async Task<object> ReadImageAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = GetClient();

        if (!_cachingOptions.Enabled)
        {
            return await client.ReadImageAsync(id, cancellationToken);
        }

        var cacheKey = $"image_{id}";

        if (_cache.TryGetValue(cacheKey, out object? cachedImage) && cachedImage != null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cachedImage;
        }

        _logger.LogDebug("Cache miss for {CacheKey}", cacheKey);
        var image = await client.ReadImageAsync(id, cancellationToken);
        _cache.Set(cacheKey, image, GetCacheEntryOptions());
        return image;
    }
}
