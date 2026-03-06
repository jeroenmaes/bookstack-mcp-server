using BookStackApiClient;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BookStackMcpServer.Services;

[McpServerToolType]
public class BookStackMcpWriteTools
{
    private readonly BookStackClientFactory _clientFactory;
    private readonly ILogger<BookStackMcpWriteTools> _logger;

    public BookStackMcpWriteTools(BookStackClientFactory clientFactory, ILogger<BookStackMcpWriteTools> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    private BookStackClient GetClient()
    {
        return _clientFactory.CreateClient();
    }

    [Description("Create a new book")]
    [McpServerTool]
    public async Task<string> CreateBookAsync(string name, string? description = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating book with name='{BookName}'", name);
            var client = GetClient();
            var args = new CreateBookArgs(name, description);
            var result = await client.CreateBookAsync(args, imgPath: null, imgName: null, cancelToken: cancellationToken);
            _logger.LogInformation("Book created successfully with ID={BookId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create book with name='{BookName}'", name);
            return JsonSerializer.Serialize(new { error = "Failed to create book", message = ex.Message, bookName = name }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Update an existing book")]
    [McpServerTool]
    public async Task<string> UpdateBookAsync(int id, string? name = null, string? description = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating book with ID={BookId}, name='{BookName}'", id, name);
            var client = GetClient();
            var args = new UpdateBookArgs(name, description);
            var result = await client.UpdateBookAsync(id, args, imgPath: null, imgName: null, cancelToken: cancellationToken);
            _logger.LogInformation("Book updated successfully with ID={BookId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update book with ID={BookId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to update book", message = ex.Message, bookId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Delete a book")]
    [McpServerTool]
    public async Task<string> DeleteBookAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting book with ID={BookId}", id);
            var client = GetClient();
            await client.DeleteBookAsync(id, cancellationToken);
            _logger.LogInformation("Book deleted successfully with ID={BookId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete book with ID={BookId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete book", message = ex.Message, bookId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Create a new chapter")]
    [McpServerTool]
    public async Task<string> CreateChapterAsync(string name, int bookId, string? description = null, int priority = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating chapter with name='{ChapterName}', bookId={BookId}", name, bookId);
            var client = GetClient();
            var args = new CreateChapterArgs(bookId, name, description, priority: priority);
            var result = await client.CreateChapterAsync(args, cancellationToken);
            _logger.LogInformation("Chapter created successfully with ID={ChapterId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create chapter with name='{ChapterName}', bookId={BookId}", name, bookId);
            return JsonSerializer.Serialize(new { error = "Failed to create chapter", message = ex.Message, chapterName = name, bookId }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Update an existing chapter")]
    [McpServerTool]
    public async Task<string> UpdateChapterAsync(int id, string? name = null, string? description = null, int? bookId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating chapter with ID={ChapterId}, name='{ChapterName}', bookId={BookId}", id, name, bookId);
            var client = GetClient();
            var args = new UpdateChapterArgs(name, description, book_id: bookId);
            var result = await client.UpdateChapterAsync(id, args, cancellationToken);
            _logger.LogInformation("Chapter updated successfully with ID={ChapterId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update chapter with ID={ChapterId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to update chapter", message = ex.Message, chapterId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Delete a chapter")]
    [McpServerTool]
    public async Task<string> DeleteChapterAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting chapter with ID={ChapterId}", id);
            var client = GetClient();
            await client.DeleteChapterAsync(id, cancellationToken);
            _logger.LogInformation("Chapter deleted successfully with ID={ChapterId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete chapter with ID={ChapterId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete chapter", message = ex.Message, chapterId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Create a new page")]
    [McpServerTool]
    public async Task<string> CreatePageAsync(string name, string content, int? bookId = null, int? chapterId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating page with name='{PageName}', bookId={BookId}, chapterId={ChapterId}", name, bookId, chapterId);
            var client = GetClient();
            var args = new CreatePageArgs(name, bookId, chapterId, html: content);
            var result = await client.CreatePageAsync(args, cancellationToken);
            _logger.LogInformation("Page created successfully with ID={PageId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create page with name='{PageName}', bookId={BookId}, chapterId={ChapterId}", name, bookId, chapterId);
            return JsonSerializer.Serialize(new { error = "Failed to create page", message = ex.Message, pageName = name, bookId, chapterId }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Update an existing page")]
    [McpServerTool]
    public async Task<string> UpdatePageAsync(int id, string? name = null, string? content = null, int? bookId = null, int? chapterId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating page with ID={PageId}, name='{PageName}', bookId={BookId}, chapterId={ChapterId}", id, name, bookId, chapterId);
            var client = GetClient();
            var args = new UpdatePageArgs(name, bookId, chapterId, html: content);
            var result = await client.UpdatePageAsync(id, args, cancellationToken);
            _logger.LogInformation("Page updated successfully with ID={PageId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update page with ID={PageId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to update page", message = ex.Message, pageId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Delete a page")]
    [McpServerTool]
    public async Task<string> DeletePageAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting page with ID={PageId}", id);
            var client = GetClient();
            await client.DeletePageAsync(id, cancellationToken);
            _logger.LogInformation("Page deleted successfully with ID={PageId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete page with ID={PageId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete page", message = ex.Message, pageId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Create a new shelf")]
    [McpServerTool]
    public async Task<string> CreateShelfAsync(string name, string? description = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating shelf with name='{ShelfName}'", name);
            var client = GetClient();
            var args = new CreateShelfArgs(name, description);
            var result = await client.CreateShelfAsync(args, imgPath: null, imgName: null, cancelToken: cancellationToken);
            _logger.LogInformation("Shelf created successfully with ID={ShelfId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create shelf with name='{ShelfName}'", name);
            return JsonSerializer.Serialize(new { error = "Failed to create shelf", message = ex.Message, shelfName = name }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Update an existing shelf")]
    [McpServerTool]
    public async Task<string> UpdateShelfAsync(int id, string? name = null, string? description = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating shelf with ID={ShelfId}, name='{ShelfName}'", id, name);
            var client = GetClient();
            var args = new UpdateShelfArgs(name, description);
            var result = await client.UpdateShelfAsync(id, args, imgPath: null, imgName: null, cancelToken: cancellationToken);
            _logger.LogInformation("Shelf updated successfully with ID={ShelfId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update shelf with ID={ShelfId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to update shelf", message = ex.Message, shelfId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Delete a shelf")]
    [McpServerTool]
    public async Task<string> DeleteShelfAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting shelf with ID={ShelfId}", id);
            var client = GetClient();
            await client.DeleteShelfAsync(id, cancellationToken);
            _logger.LogInformation("Shelf deleted successfully with ID={ShelfId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete shelf with ID={ShelfId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete shelf", message = ex.Message, shelfId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Create a new user")]
    [McpServerTool]
    public async Task<string> CreateUserAsync(string name, string email, string? password = null, string? roleIds = null, bool? sendInvite = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating user with name='{UserName}', email='{Email}'", name, email);
            var client = GetClient();
            var roles = ParseRoleIds(roleIds);
            var args = new CreateUserArgs(name, email, null, null, password, roles, sendInvite);
            var result = await client.CreateUserAsync(args, cancellationToken);
            _logger.LogInformation("User created successfully with ID={UserId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user with name='{UserName}', email='{Email}'", name, email);
            return JsonSerializer.Serialize(new { error = "Failed to create user", message = ex.Message, userName = name, email }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Update an existing user")]
    [McpServerTool]
    public async Task<string> UpdateUserAsync(int id, string? name = null, string? email = null, string? password = null, string? roleIds = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with ID={UserId}, name='{UserName}'", id, name);
            var client = GetClient();
            var roles = ParseRoleIds(roleIds);
            var args = new UpdateUserArgs(name, email, null, null, password, roles, null);
            var result = await client.UpdateUserAsync(id, args, cancellationToken);
            _logger.LogInformation("User updated successfully with ID={UserId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user with ID={UserId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to update user", message = ex.Message, userId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Delete a user")]
    [McpServerTool]
    public async Task<string> DeleteUserAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID={UserId}", id);
            var client = GetClient();
            await client.DeleteUserAsync(id, cancellationToken);
            _logger.LogInformation("User deleted successfully with ID={UserId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete user with ID={UserId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete user", message = ex.Message, userId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Create a new role")]
    [McpServerTool]
    public async Task<string> CreateRoleAsync(string displayName, string? description = null, bool? mfaEnforced = null, string? permissions = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating role with displayName='{DisplayName}'", displayName);
            var client = GetClient();
            var permissionList = ParsePermissions(permissions);
            var args = new CreateRoleArgs(displayName, description, mfaEnforced, null, permissionList);
            var result = await client.CreateRoleAsync(args, cancellationToken);
            _logger.LogInformation("Role created successfully with ID={RoleId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create role with displayName='{DisplayName}'", displayName);
            return JsonSerializer.Serialize(new { error = "Failed to create role", message = ex.Message, displayName }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Update an existing role")]
    [McpServerTool]
    public async Task<string> UpdateRoleAsync(int id, string? displayName = null, string? description = null, bool? mfaEnforced = null, string? permissions = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating role with ID={RoleId}, displayName='{DisplayName}'", id, displayName);
            var client = GetClient();
            var permissionList = ParsePermissions(permissions);
            var args = new UpdateRoleArgs(displayName, description, mfaEnforced, null, permissionList);
            var result = await client.UpdateRoleAsync(id, args, cancellationToken);
            _logger.LogInformation("Role updated successfully with ID={RoleId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update role with ID={RoleId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to update role", message = ex.Message, roleId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Delete a role")]
    [McpServerTool]
    public async Task<string> DeleteRoleAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting role with ID={RoleId}", id);
            var client = GetClient();
            await client.DeleteRoleAsync(id, cancellationToken);
            _logger.LogInformation("Role deleted successfully with ID={RoleId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete role with ID={RoleId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete role", message = ex.Message, roleId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Create a new link attachment on a page")]
    [McpServerTool]
    public async Task<string> CreateAttachmentAsync(string name, int uploadedTo, string link, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating link attachment with name='{AttachmentName}', pageId={PageId}", name, uploadedTo);
            var client = GetClient();
            var args = new CreateLinkAttachmentArgs(name, uploadedTo, link);
            var result = await client.CreateLinkAttachmentAsync(args, cancellationToken);
            _logger.LogInformation("Attachment created successfully with ID={AttachmentId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create attachment with name='{AttachmentName}', pageId={PageId}", name, uploadedTo);
            return JsonSerializer.Serialize(new { error = "Failed to create attachment", message = ex.Message, attachmentName = name, uploadedTo }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Update an existing link attachment")]
    [McpServerTool]
    public async Task<string> UpdateAttachmentAsync(int id, string? name = null, string? link = null, int? uploadedTo = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating attachment with ID={AttachmentId}", id);
            var client = GetClient();
            var args = new UpdateLinkAttachmentArgs(name, uploadedTo.HasValue ? (long?)uploadedTo.Value : null, link);
            var result = await client.UpdateLinkAttachmentAsync(id, args, cancellationToken);
            _logger.LogInformation("Attachment updated successfully with ID={AttachmentId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update attachment with ID={AttachmentId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to update attachment", message = ex.Message, attachmentId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Delete an attachment")]
    [McpServerTool]
    public async Task<string> DeleteAttachmentAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting attachment with ID={AttachmentId}", id);
            var client = GetClient();
            await client.DeleteAttachmentAsync(id, cancellationToken);
            _logger.LogInformation("Attachment deleted successfully with ID={AttachmentId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete attachment with ID={AttachmentId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete attachment", message = ex.Message, attachmentId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Restore a deleted item from the recycle bin")]
    [McpServerTool]
    public async Task<string> RestoreRecycleItemAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Restoring recycle bin item with ID={DeletionId}", id);
            var client = GetClient();
            var result = await client.RestoreRecycleItemAsync(id, cancellationToken);
            _logger.LogInformation("Recycle bin item restored successfully, restore_count={RestoreCount}", result.restore_count);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore recycle bin item with ID={DeletionId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to restore recycle bin item", message = ex.Message, deletionId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Permanently delete an item from the recycle bin")]
    [McpServerTool]
    public async Task<string> DestroyRecycleItemAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Permanently deleting recycle bin item with ID={DeletionId}", id);
            var client = GetClient();
            await client.DestroyRecycleItemAsync(id, cancellationToken);
            _logger.LogInformation("Recycle bin item permanently deleted with ID={DeletionId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to permanently delete recycle bin item with ID={DeletionId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to permanently delete recycle bin item", message = ex.Message, deletionId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    private static IReadOnlyList<long>? ParseRoleIds(string? roleIds)
    {
        if (string.IsNullOrWhiteSpace(roleIds))
            return null;

        var ids = new List<long>();
        foreach (var part in roleIds.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (long.TryParse(part, out var roleId))
                ids.Add(roleId);
        }
        return ids.Count > 0 ? ids : null;
    }

    private static IReadOnlyList<string>? ParsePermissions(string? permissions)
    {
        if (string.IsNullOrWhiteSpace(permissions))
            return null;

        var perms = permissions
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
        return perms.Count > 0 ? perms : null;
    }

    [Description("Upload an image to the gallery. imageBase64 must be base64-encoded image content. type should be 'gallery' or 'drawio'. uploadedTo is the optional page ID to associate the image with (use 0 or omit for no page association).")]
    [McpServerTool]
    public async Task<string> CreateImageAsync(string name, string imageBase64, string type = "gallery", int uploadedTo = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating image with name='{ImageName}', type='{Type}', uploadedTo={UploadedTo}", name, type, uploadedTo);
            var client = GetClient();
            var imageBytes = Convert.FromBase64String(imageBase64);
            var args = new CreateImageArgs(uploadedTo, type, name);
            var result = await client.CreateImageAsync(args, imageBytes, $"{name}.png", cancellationToken);
            _logger.LogInformation("Image created successfully with ID={ImageId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create image with name='{ImageName}'", name);
            return JsonSerializer.Serialize(new { error = "Failed to create image", message = ex.Message, imageName = name }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Update an existing image's name. To replace image content, provide imageBase64 with the new base64-encoded image. At least one of name or imageBase64 must be provided.")]
    [McpServerTool]
    public async Task<string> UpdateImageAsync(int id, string? name = null, string? imageBase64 = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (name is null && string.IsNullOrEmpty(imageBase64))
            {
                return JsonSerializer.Serialize(new { error = "At least one of name or imageBase64 must be provided" }, new JsonSerializerOptions { WriteIndented = true });
            }

            _logger.LogInformation("Updating image with ID={ImageId}", id);
            var client = GetClient();
            var args = new UpdateImageArgs(name ?? string.Empty);
            if (!string.IsNullOrEmpty(imageBase64))
            {
                var imageBytes = Convert.FromBase64String(imageBase64);
                var result = await client.UpdateImageAsync(id, args, imageBytes, "image.png", cancellationToken);
                _logger.LogInformation("Image updated successfully with ID={ImageId}", result.id);
                return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            }
            else
            {
                var result = await client.UpdateImageAsync(id, args, path: null, fileName: null, cancellationToken);
                _logger.LogInformation("Image updated successfully with ID={ImageId}", result.id);
                return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update image with ID={ImageId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to update image", message = ex.Message, imageId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Delete an image from the gallery")]
    [McpServerTool]
    public async Task<string> DeleteImageAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting image with ID={ImageId}", id);
            var client = GetClient();
            await client.DeleteImageAsync(id, cancellationToken);
            _logger.LogInformation("Image deleted successfully with ID={ImageId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete image with ID={ImageId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete image", message = ex.Message, imageId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
