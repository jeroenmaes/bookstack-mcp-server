using BookStackApiClient;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BookStackMcpServer.Services;

[McpServerToolType]
public class BookStackMcpTools
{
    private readonly CachedBookStackClient _client;
    private readonly ILogger<BookStackMcpTools> _logger;

    public BookStackMcpTools(CachedBookStackClient client, ILogger<BookStackMcpTools> logger)
    {
        _client = client;
        _logger = logger;
    }

    // Books management - simplified version
    [Description("List all books")]
    [McpServerTool]
    public async Task<string> ListBooksAsync(int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Listing books with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListBooksAsync(listing, cancellationToken);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list books with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list books", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Get book details by ID")]
    [McpServerTool]
    public async Task<string> GetBookAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting book with ID={BookId}", id);
            var book = await _client.ReadBookAsync(id, cancellationToken);
            return JsonSerializer.Serialize(book, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get book with ID={BookId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get book", message = ex.Message, bookId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Chapters management - simplified version
    [Description("List all chapters")]
    [McpServerTool]
    public async Task<string> ListChaptersAsync(int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Listing chapters with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListChaptersAsync(listing, cancellationToken);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list chapters with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list chapters", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Get chapter details by ID")]
    [McpServerTool]
    public async Task<string> GetChapterAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting chapter with ID={ChapterId}", id);
            var chapter = await _client.ReadChapterAsync(id, cancellationToken);
            return JsonSerializer.Serialize(chapter, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get chapter with ID={ChapterId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get chapter", message = ex.Message, chapterId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Pages management - simplified version
    [Description("List all pages")]
    [McpServerTool]
    public async Task<string> ListPagesAsync(int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Listing pages with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListPagesAsync(listing, cancellationToken);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list pages with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list pages", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Get page details by ID")]
    [McpServerTool]
    public async Task<string> GetPageAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting page with ID={PageId}", id);
            var page = await _client.ReadPageAsync(id, cancellationToken);
            return JsonSerializer.Serialize(page, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get page with ID={PageId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get page", message = ex.Message, pageId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Shelves management - simplified version
    [Description("List all shelves")]
    [McpServerTool]
    public async Task<string> ListShelvesAsync(int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Listing shelves with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListShelvesAsync(listing, cancellationToken);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list shelves with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list shelves", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Get shelf details by ID")]
    [McpServerTool]
    public async Task<string> GetShelfAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting shelf with ID={ShelfId}", id);
            var shelf = await _client.ReadShelfAsync(id, cancellationToken);
            return JsonSerializer.Serialize(shelf, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get shelf with ID={ShelfId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get shelf", message = ex.Message, shelfId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Users management - simplified version
    [Description("List all users")]
    [McpServerTool]
    public async Task<string> ListUsersAsync(int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Listing users with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListUsersAsync(listing, cancellationToken);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list users with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list users", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Get user details by ID")]
    [McpServerTool]
    public async Task<string> GetUserAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting user with ID={UserId}", id);
            var user = await _client.ReadUserAsync(id, cancellationToken);
            return JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user with ID={UserId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get user", message = ex.Message, userId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Search functionality
    [Description("Search across all BookStack content (books, chapters, pages)")]
    [McpServerTool]
    public async Task<string> SearchAllAsync(string query, int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching all content with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            var args = new SearchArgs(query, null, null);
            var response = await _client.SearchAsync(args, cancellationToken);
            
            // Cast to dynamic to access properties
            dynamic dynamicResponse = response;
            var allBooks = ((IEnumerable<dynamic>)dynamicResponse.books()).ToList();
            var allChapters = ((IEnumerable<dynamic>)dynamicResponse.chapters()).ToList();
            var allPages = ((IEnumerable<dynamic>)dynamicResponse.pages()).ToList();
            var allShelves = ((IEnumerable<dynamic>)dynamicResponse.shelves()).ToList();
            
            var totalResults = allBooks.Count + allChapters.Count + allPages.Count + allShelves.Count;
            
            var results = new
            {
                query = query,
                total = totalResults,
                books = allBooks.Skip(offset).Take(count).ToList(),
                chapters = allChapters.Skip(offset).Take(count).ToList(),
                pages = allPages.Skip(offset).Take(count).ToList(),
                shelves = allShelves.Skip(offset).Take(count).ToList()
            };
            
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search all content with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search content", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Search for books by name or description")]
    [McpServerTool]
    public async Task<string> SearchBooksAsync(string query, int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching books with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            var args = new SearchArgs(query, null, null);
            var response = await _client.SearchAsync(args, cancellationToken);
            
            // Cast to dynamic to access properties
            dynamic dynamicResponse = response;
            var booksList = ((IEnumerable<dynamic>)dynamicResponse.books()).Skip(offset).Take(count).ToList();
            var results = new
            {
                query = query,
                total = ((IEnumerable<dynamic>)dynamicResponse.books()).Count(),
                data = booksList
            };
            
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search books with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search books", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Search for chapters by name or description")]
    [McpServerTool]
    public async Task<string> SearchChaptersAsync(string query, int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching chapters with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            var args = new SearchArgs(query, null, null);
            var response = await _client.SearchAsync(args, cancellationToken);
            
            // Cast to dynamic to access properties
            dynamic dynamicResponse = response;
            var chaptersList = ((IEnumerable<dynamic>)dynamicResponse.chapters()).Skip(offset).Take(count).ToList();
            var results = new
            {
                query = query,
                total = ((IEnumerable<dynamic>)dynamicResponse.chapters()).Count(),
                data = chaptersList
            };
            
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search chapters with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search chapters", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Search for pages by name or content")]
    [McpServerTool]
    public async Task<string> SearchPagesAsync(string query, int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching pages with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            var args = new SearchArgs(query, null, null);
            var response = await _client.SearchAsync(args, cancellationToken);
            
            // Cast to dynamic to access properties
            dynamic dynamicResponse = response;
            var pagesList = ((IEnumerable<dynamic>)dynamicResponse.pages()).Skip(offset).Take(count).ToList();
            var results = new
            {
                query = query,
                total = ((IEnumerable<dynamic>)dynamicResponse.pages()).Count(),
                data = pagesList
            };
            
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search pages with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search pages", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Search for shelves by name or description")]
    [McpServerTool]
    public async Task<string> SearchShelvesAsync(string query, int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching shelves with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            var args = new SearchArgs(query, null, null);
            var response = await _client.SearchAsync(args, cancellationToken);
            
            // Cast to dynamic to access properties
            dynamic dynamicResponse = response;
            var shelvesList = ((IEnumerable<dynamic>)dynamicResponse.shelves()).Skip(offset).Take(count).ToList();
            var results = new
            {
                query = query,
                total = ((IEnumerable<dynamic>)dynamicResponse.shelves()).Count(),
                data = shelvesList
            };
            
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search shelves with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search shelves", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Search for users by name or email")]
    [McpServerTool]
    public async Task<string> SearchUsersAsync(string query, int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            // Note: The new API's search doesn't include users, so we use list with filters
            var listing = new ListingOptions(offset: offset, count: count, filters: new[] { new Filter("name:like", $"%{query}%") });
            var response = await _client.ListUsersAsync(listing, cancellationToken);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search users with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search users", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Advanced search with custom filters")]
    [McpServerTool]
    public async Task<string> AdvancedSearchAsync(string entityType, string field, string value, string operatorType = "like", int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Advanced search for entityType='{EntityType}', field='{Field}', value='{Value}', operator='{Operator}'", entityType, field, value, operatorType);
            var filter = new Filter($"{field}:{operatorType}", value);
            var listing = new ListingOptions(offset: offset, count: count, filters: new[] { filter });
            
            // Determine entity type and search accordingly
            switch (entityType.ToLower())
            {
                case "book":
                case "books":
                    var bookResponse = await _client.ListBooksAsync(listing, cancellationToken);
                    return JsonSerializer.Serialize(bookResponse, new JsonSerializerOptions { WriteIndented = true });
                    
                case "chapter":
                case "chapters":
                    var chapterResponse = await _client.ListChaptersAsync(listing, cancellationToken);
                    return JsonSerializer.Serialize(chapterResponse, new JsonSerializerOptions { WriteIndented = true });
                    
                case "page":
                case "pages":
                    var pageResponse = await _client.ListPagesAsync(listing, cancellationToken);
                    return JsonSerializer.Serialize(pageResponse, new JsonSerializerOptions { WriteIndented = true });
                    
                case "shelf":
                case "shelves":
                    var shelfResponse = await _client.ListShelvesAsync(listing, cancellationToken);
                    return JsonSerializer.Serialize(shelfResponse, new JsonSerializerOptions { WriteIndented = true });
                    
                case "user":
                case "users":
                    var userResponse = await _client.ListUsersAsync(listing, cancellationToken);
                    return JsonSerializer.Serialize(userResponse, new JsonSerializerOptions { WriteIndented = true });
                    
                default:
                    _logger.LogWarning("Unknown entity type requested: {EntityType}", entityType);
                    return JsonSerializer.Serialize(new { error = $"Unknown entity type: {entityType}. Supported types: book, chapter, page, shelf, user" }, new JsonSerializerOptions { WriteIndented = true });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to perform advanced search for entityType='{EntityType}', field='{Field}'", entityType, field);
            return JsonSerializer.Serialize(new { error = "Failed to perform advanced search", message = ex.Message, entityType, field }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Roles management
    [Description("List all roles")]
    [McpServerTool]
    public async Task<string> ListRolesAsync(int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Listing roles with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListRolesAsync(listing, cancellationToken);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list roles with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list roles", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Get role details by ID")]
    [McpServerTool]
    public async Task<string> GetRoleAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting role with ID={RoleId}", id);
            var role = await _client.ReadRoleAsync(id, cancellationToken);
            return JsonSerializer.Serialize(role, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get role with ID={RoleId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get role", message = ex.Message, roleId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Attachments management
    [Description("List all attachments")]
    [McpServerTool]
    public async Task<string> ListAttachmentsAsync(int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Listing attachments with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListAttachmentsAsync(listing, cancellationToken);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list attachments with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list attachments", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Get attachment details by ID")]
    [McpServerTool]
    public async Task<string> GetAttachmentAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting attachment with ID={AttachmentId}", id);
            var attachment = await _client.ReadAttachmentAsync(id, cancellationToken);
            return JsonSerializer.Serialize(attachment, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get attachment with ID={AttachmentId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get attachment", message = ex.Message, attachmentId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Recycle bin management
    [Description("List items in the recycle bin")]
    [McpServerTool]
    public async Task<string> ListRecycleBinAsync(int offset = 0, int count = 500, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Listing recycle bin with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListRecycleBinAsync(listing, cancellationToken);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list recycle bin with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list recycle bin", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Audit log
    [Description("List audit log entries")]
    [McpServerTool]
    public async Task<string> ListAuditLogAsync(int offset = 0, int count = 100, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Listing audit log with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListAuditLogAsync(listing, cancellationToken);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list audit log with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list audit log", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Content permissions
    [Description("Read content permissions for a book, chapter, page, or shelf. contentType must be one of: book, chapter, page, shelf")]
    [McpServerTool]
    public async Task<string> ReadContentPermissionsAsync(string contentType, int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Reading permissions for contentType='{ContentType}', ID={ContentId}", contentType, id);
            var permissions = await _client.ReadContentPermissionsAsync(contentType, id, cancellationToken);
            return JsonSerializer.Serialize(permissions, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read permissions for contentType='{ContentType}', ID={ContentId}", contentType, id);
            return JsonSerializer.Serialize(new { error = "Failed to read content permissions", message = ex.Message, contentType, contentId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Export tools
    [Description("Export a book in the specified format. Supported formats: html, markdown, plaintext")]
    [McpServerTool]
    public async Task<string> ExportBookAsync(int id, string format = "html", CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Exporting book with ID={BookId}, format='{Format}'", id, format);
            string content = format.ToLowerInvariant() switch
            {
                "markdown" or "md" => await _client.ExportBookMarkdownAsync(id, cancellationToken),
                "plaintext" or "plain" or "text" => await _client.ExportBookPlainAsync(id, cancellationToken),
                _ => await _client.ExportBookHtmlAsync(id, cancellationToken)
            };
            return JsonSerializer.Serialize(new { id, format, content }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export book with ID={BookId}, format='{Format}'", id, format);
            return JsonSerializer.Serialize(new { error = "Failed to export book", message = ex.Message, bookId = id, format }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Export a chapter in the specified format. Supported formats: html, markdown, plaintext")]
    [McpServerTool]
    public async Task<string> ExportChapterAsync(int id, string format = "html", CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Exporting chapter with ID={ChapterId}, format='{Format}'", id, format);
            string content = format.ToLowerInvariant() switch
            {
                "markdown" or "md" => await _client.ExportChapterMarkdownAsync(id, cancellationToken),
                "plaintext" or "plain" or "text" => await _client.ExportChapterPlainAsync(id, cancellationToken),
                _ => await _client.ExportChapterHtmlAsync(id, cancellationToken)
            };
            return JsonSerializer.Serialize(new { id, format, content }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export chapter with ID={ChapterId}, format='{Format}'", id, format);
            return JsonSerializer.Serialize(new { error = "Failed to export chapter", message = ex.Message, chapterId = id, format }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Export a page in the specified format. Supported formats: html, markdown, plaintext")]
    [McpServerTool]
    public async Task<string> ExportPageAsync(int id, string format = "html", CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Exporting page with ID={PageId}, format='{Format}'", id, format);
            string content = format.ToLowerInvariant() switch
            {
                "markdown" or "md" => await _client.ExportPageMarkdownAsync(id, cancellationToken),
                "plaintext" or "plain" or "text" => await _client.ExportPagePlainAsync(id, cancellationToken),
                _ => await _client.ExportPageHtmlAsync(id, cancellationToken)
            };
            return JsonSerializer.Serialize(new { id, format, content }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export page with ID={PageId}, format='{Format}'", id, format);
            return JsonSerializer.Serialize(new { error = "Failed to export page", message = ex.Message, pageId = id, format }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}