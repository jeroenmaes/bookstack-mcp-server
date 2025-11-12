# BookStack MCP Server

An ASP.NET Core 10 Model Context Protocol (MCP) server that provides tools for interacting with BookStack wiki software via its API.

## Features

This MCP server implements tools for all major BookStack API endpoints:

**Note:** Write operations (create/delete) are disabled by default. Set `BookStack:EnableWrite` to `true` to enable them.

### Books Management
- `list_books` - List all books with pagination
- `get_book` - Get detailed information about a specific book
- `create_book` - Create a new book (requires `EnableWrite: true`)
- `delete_book` - Delete a book (requires `EnableWrite: true`)

### Chapters Management  
- `list_chapters` - List all chapters with pagination
- `get_chapter` - Get detailed information about a specific chapter
- `create_chapter` - Create a new chapter in a book (requires `EnableWrite: true`)
- `delete_chapter` - Delete a chapter (requires `EnableWrite: true`)

### Pages Management
- `list_pages` - List all pages with pagination
- `get_page` - Get detailed information about a specific page
- `create_page` - Create a new page in a book or chapter (requires `EnableWrite: true`)
- `delete_page` - Delete a page (requires `EnableWrite: true`)

### Shelves Management
- `list_shelves` - List all shelves with pagination
- `get_shelf` - Get detailed information about a specific shelf
- `create_shelf` - Create a new shelf (requires `EnableWrite: true`)
- `delete_shelf` - Delete a shelf (requires `EnableWrite: true`)

### Users Management
- `list_users` - List all users with pagination
- `get_user` - Get detailed information about a specific user

### Search Functionality
- `search_all` - Search across all BookStack content (books, chapters, pages)
- `search_books` - Search for books by name or description
- `search_chapters` - Search for chapters by name or description  
- `search_pages` - Search for pages by name or content
- `search_shelves` - Search for shelves by name or description
- `search_users` - Search for users by name or email
- `advanced_search` - Advanced search with custom filters and operators

## Setup

### Prerequisites
- .NET 10 SDK
- BookStack instance with API access enabled
- BookStack API token credentials (Token ID and Token Secret)

### Configuration

1. Clone the repository:
```bash
git clone https://github.com/jeroenmaes/dotnet-bookstack-mcp-server.git
cd dotnet-bookstack-mcp-server
```

2. Configure your BookStack base URL in `BookStackMcpServer/appsettings.json`:
```json
{
  "BookStack": {
    "BaseUrl": "https://your-bookstack-instance.com"
  }
}
```

3. Run the application:
```bash
cd BookStackMcpServer
dotnet run
```

### Authentication

**Important:** This MCP server uses per-request authentication using Bearer tokens. Each request must include BookStack API credentials in the `Authorization` header.

Use the standard `Authorization` header with a Bearer token containing your credentials separated by a colon:

```bash
Authorization: Bearer <token_id>:<token_secret>
```

Example:
```bash
curl -X POST http://server:5230/mcp/v1 \
  -H "Authorization: Bearer your-token-id:your-token-secret" \
  -H "Content-Type: application/json" \
  -d '{"jsonrpc":"2.0","method":"tools/list","id":1}'
```

**Token Format:**

The server requires that the Bearer token contains a colon (`:`) to separate the Token ID and Token Secret. Both parts must be non-empty. The actual format and validation of the token is handled by the BookStack API itself.

This design allows multiple users to use the same MCP server instance with their own BookStack credentials.

### Docker Deployment

The application includes Docker support for easy deployment:

#### Using Docker Compose (Recommended)

1. Copy the environment template:
```bash
cp .env.example .env
```

2. Edit `.env` file with your BookStack base URL:
```bash
BOOKSTACK_BASE_URL=https://your-bookstack-instance.com
```

3. Build and run with Docker Compose:
```bash
docker-compose up -d
```

**Note:** When using Docker, you still need to provide BookStack credentials in request headers for each API call.

#### Using Docker directly

1. Build the Docker image:
```bash
docker build -t bookstack-mcp-server .
```

2. Run the container:
```bash
docker run -d \
  -p 8080:8080 \
  -e BookStack__BaseUrl=https://your-bookstack-instance.com \
  --name bookstack-mcp-server \
  bookstack-mcp-server
```

#### GitHub Container Registry

Pre-built images are available from GitHub Container Registry:

```bash
# Pull the latest image
docker pull ghcr.io/jeroenmaes/dotnet-bookstack-mcp-server:latest

# Pull a specific build number
docker pull ghcr.io/jeroenmaes/dotnet-bookstack-mcp-server:build-123

# Pull a specific version tag
docker pull ghcr.io/jeroenmaes/dotnet-bookstack-mcp-server:v1.0.0
```

## MCP Protocol

The server implements the Model Context Protocol using the official C# SDK. It exposes an MCP endpoint that can be used by MCP-compatible clients to interact with BookStack.

### Authentication

All MCP requests must include BookStack API credentials using Bearer token authentication:

```
Authorization: Bearer <token_id>:<token_secret>
```

The server validates these credentials on each request and uses them to authenticate with your BookStack instance. This allows multiple users to safely share the same MCP server while using their own BookStack credentials.

### Read-Only Mode (Default)

By default, the server operates in read-only mode for safety. Write operations (`create_*` and `delete_*` tools) are disabled unless explicitly enabled.

**To enable write operations:**

In `appsettings.json`:
```json
{
  "BookStack": {
    "BaseUrl": "https://your-bookstack-instance.com",
    "EnableWrite": true
  }
}
```

Or via environment variables:
```bash
BookStack__EnableWrite=true
```

**Docker example with write enabled:**
```bash
docker run -d \
  -p 8080:8080 \
  -e BookStack__BaseUrl=https://your-bookstack-instance.com \
  -e BookStack__EnableWrite=true \
  --name bookstack-mcp-server \
  bookstack-mcp-server
```

**Notes:**
- When `EnableWrite` is `false` (default), only read operations (list, get, search) are available
- When `EnableWrite` is `true`, create and delete operations become available
- This provides an extra layer of protection against accidental modifications to your BookStack content
- Write permissions are still subject to the BookStack API token's actual permissions

### HTTP Throttling

The server includes built-in HTTP rate limiting to protect against abuse and excessive requests. Rate limiting is applied per IP address using a fixed window algorithm.

**Configuration:**

In `appsettings.json`:
```json
{
  "Throttling": {
    "Enabled": true,
    "PermitLimit": 100,
    "WindowSeconds": 60,
    "QueueLimit": 0
  }
}
```

Or via environment variables:
```bash
Throttling__Enabled=true
Throttling__PermitLimit=100
Throttling__WindowSeconds=60
Throttling__QueueLimit=0
```

**Docker example:**
```bash
docker run -d \
  -p 8080:8080 \
  -e BookStack__BaseUrl=https://your-bookstack-instance.com \
  -e BookStack__TokenId=your-token-id \
  -e BookStack__TokenSecret=your-token-secret \
  -e Throttling__Enabled=true \
  -e Throttling__PermitLimit=100 \
  -e Throttling__WindowSeconds=60 \
  --name bookstack-mcp-server \
  bookstack-mcp-server
```

**Configuration Options:**
- `Enabled` - Enable or disable rate limiting (default: `true`)
- `PermitLimit` - Maximum number of requests allowed per time window (default: `100`)
- `WindowSeconds` - Time window in seconds for rate limiting (default: `60`)
- `QueueLimit` - Number of requests to queue when limit is reached (default: `0` - no queuing)

**Notes:**
- Rate limiting is applied per IP address
- Health check endpoints (`/health`, `/health/live`, `/health/ready`) are not rate limited
- When the limit is exceeded, the server returns a `429 Too Many Requests` response
- Set `Enabled` to `false` to disable throttling entirely

### In-Memory Caching

The server includes built-in in-memory caching for BookStack API responses to improve performance and reduce load on the BookStack server. Caching is applied to all read operations (list, get, search).

**Configuration:**

In `appsettings.json`:
```json
{
  "Caching": {
    "Enabled": true,
    "AbsoluteExpirationMinutes": 5,
    "SlidingExpirationMinutes": 2
  }
}
```

Or via environment variables:
```bash
Caching__Enabled=true
Caching__AbsoluteExpirationMinutes=5
Caching__SlidingExpirationMinutes=2
```

**Docker example:**
```bash
docker run -d \
  -p 8080:8080 \
  -e BookStack__BaseUrl=https://your-bookstack-instance.com \
  -e BookStack__TokenId=your-token-id \
  -e BookStack__TokenSecret=your-token-secret \
  -e Caching__Enabled=true \
  -e Caching__AbsoluteExpirationMinutes=5 \
  -e Caching__SlidingExpirationMinutes=2 \
  --name bookstack-mcp-server \
  bookstack-mcp-server
```

**Configuration Options:**
- `Enabled` - Enable or disable caching (default: `true`)
- `AbsoluteExpirationMinutes` - Maximum time an item stays in cache regardless of access (default: `5` minutes)
- `SlidingExpirationMinutes` - Time window of inactivity before an item expires (default: `2` minutes)

**Notes:**
- Caching significantly improves response times for repeated requests
- Each unique request (different parameters) is cached separately
- Absolute expiration ensures data freshness by forcing cache refresh after a maximum time
- Sliding expiration keeps frequently accessed items in cache longer
- Set `Enabled` to `false` to disable caching entirely
- Write operations are not cached and automatically invalidate relevant cache entries

### Health Check Endpoints

The server provides ASP.NET Core health check endpoints for monitoring:

- `GET /health` - Comprehensive health check with detailed status information
- `GET /health/ready` - Readiness check (includes BookStack API dependency check)
- `GET /health/live` - Liveness check (returns 200 if the application is running)

Check application health:
```bash
# Liveness check (app is running)
curl http://localhost:8080/health/live

# Readiness check (app is ready to serve requests, including BookStack API)
curl http://localhost:8080/health/ready

# Detailed health information
curl http://localhost:8080/health
```

## BookStack API Setup

1. In your BookStack instance, go to Settings → Users → API Tokens
2. Create a new API token with the required permissions
3. Use the Token ID and Token Secret in your configuration

## Dependencies

- **ASP.NET Core 10** - Web framework
- **ModelContextProtocol.AspNetCore** (0.4.0-preview.2) - MCP server implementation
- **BookStackApiClient** (25.7.0-lib.2) - BookStack API client library

## License

MIT License - see LICENSE file for details.
