# The Silicon Post - Copilot Instructions

## Project Overview

This is a **WordPress to C# migration** project - rebuilding a tech blog using **Umbraco CMS v16.2** on **.NET 9**. The goal is to replace WordPress with a modern, performant C# CMS solution while maintaining/improving SEO, content management capabilities, and site functionality.

### Why Umbraco?

- Modern .NET-based CMS (no PHP/WordPress overhead)
- Familiar backoffice UI for content editors
- Full control over code, performance, and architecture
- Built-in content versioning and media management
- Better security and performance than WordPress

## Architecture Patterns

### Strongly-Typed Content Models

All Umbraco content types use **strongly-typed wrappers** inheriting from `PublishedContentWrapped`:

```csharp
public class BlogPost : PublishedContentWrapped
{
    public BlogPost(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
        : base(content, publishedValueFallback) { }

    public string Title => this.Value<string>("title") ?? this.Name;
}
```

- Always pass both `IPublishedContent` and `IPublishedValueFallback` to constructors
- Property aliases are camelCase (e.g., `"featuredImage"`, `"publishDate"`)
- Use null coalescing for safe defaults (`?? this.Name`, `?? Enumerable.Empty<T>()`)

### Controller Pattern

Custom functionality uses **SurfaceController** (not ApiController):

```csharp
public class RssFeedController : SurfaceController
{
    public RssFeedController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        IConfiguration configuration)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _configuration = configuration;
    }
}
```

- Use `[Route("path")]` attributes for custom routes
- Access content via `UmbracoContext.Content.GetAtRoot()`
- Query by document type: `.DescendantsOrSelfOfType("blogPost")`

### SEO Implementation

`SeoHelper` generates comprehensive SEO metadata:

- OpenGraph tags for social sharing
- Twitter Card metadata
- Schema.org JSON-LD structured data (BlogPosting type)
- Canonical URLs and meta descriptions
- Always escape JSON strings and strip HTML from excerpts

## Key Configuration

### Site Settings (appsettings.json)

```json
"SiteSettings": {
  "Url": "https://thesiliconpost.com",
  "Name": "The Silicon Post",
  "Description": "Tech blog covering AI, Cloud, .NET, and modern software development"
}
```

### Content Architecture (WordPress → Umbraco Mapping)

- **blogPost** → Replaces WordPress "Post" with `publishDate`, `excerpt`, `featuredImage`, `categories`, `tags`, SEO fields
- **Author** → Custom author profiles (replaces WP user profiles) with social links (Twitter, GitHub, LinkedIn) and avatar
- **Category/Tag** → Direct equivalents to WordPress taxonomy with names, descriptions, and slugs
- **No plugins needed** - SEO, RSS, sitemap are built directly into the application

### WordPress Feature Parity

✅ **Implemented:**

- RSS feed generation (`/feed/rss`) - replaces WordPress RSS
- XML sitemap (`/sitemap.xml`) - replaces Yoast/Rank Math sitemap
- SEO metadata (OpenGraph, Twitter Cards, Schema.org) - replaces Yoast/Rank Math
- Content categories and tags
- Featured images
- Author profiles

🚧 **To Implement:**

- Content migration from WordPress (consider WP REST API export)
- URL redirects from old WordPress structure
- Comments system (if needed)
- Search functionality
- Related posts
- Custom post templates/layouts

## Development Workflows

### Running the Project

```bash
dotnet run
```

- Umbraco backoffice: `/umbraco`
- RSS feed: `/feed/rss`
- Sitemap: `/sitemap.xml`

### Key Files to Reference

- `Program.cs`: Minimal Umbraco bootstrap (don't modify unless necessary)
- `TheSiliconPost.csproj`: Razor compilation disabled (`RazorCompileOnBuild=false`) for Umbraco compatibility
- `Controllers/`: Custom routes for RSS/sitemap
- `Models/`: Strongly-typed content wrappers
- `Helpers/SeoHelper.cs`: SEO metadata generation

## Critical Conventions

1. **Never** use dynamic content access - always create typed models
2. **Always** handle null content collections with `?? Enumerable.Empty<T>()`
3. **Always** XML-escape URLs in sitemap generation
4. Use `IConfiguration` for site-wide settings, not hardcoded values
5. RSS feeds use `SyndicationFeed` from `System.ServiceModel.Syndication`
6. Compression is **disabled** in csproj for Umbraco backoffice performance

## Common Tasks

**Adding a new content model:**

1. Create class inheriting `PublishedContentWrapped`
2. Add constructor accepting `IPublishedContent` and `IPublishedValueFallback`
3. Define properties using `this.Value<T>("propertyAlias")`

**Adding a new controller:**

1. Inherit from `SurfaceController`
2. Use full constructor injection (see `RssFeedController` example)
3. Add `[Route]` attributes for custom URLs
4. Access content via `UmbracoContext.Content`

**Querying content:**

```csharp
var posts = UmbracoContext.Content.GetAtRoot()
    .DescendantsOrSelfOfType("blogPost")
    .OrderByDescending(x => x.Value<DateTime>("publishDate"))
    .Take(50);
```

## WordPress Migration Strategy

### Content Export Options

1. **WordPress REST API** (Recommended)

   - Export posts: `GET /wp-json/wp/v2/posts`
   - Export categories: `GET /wp-json/wp/v2/categories`
   - Export tags: `GET /wp-json/wp/v2/tags`
   - Export media: `GET /wp-json/wp/v2/media`

2. **XML Export**

   - Tools → Export → All content
   - Parse XML with C# `XDocument`

3. **Direct Database Export**
   - Export `wp_posts`, `wp_terms`, `wp_postmeta` tables
   - Create migration script to transform and import

### URL Mapping Strategy

WordPress typically uses patterns like:

- `/{year}/{month}/{day}/{slug}/` (default)
- `/{category}/{slug}/`
- `/{slug}/`

Umbraco strategy:

- Use same slug structure in Umbraco
- Create `UrlRewriteController` or `IContentFinder` for legacy URL support
- Generate 301 redirects map from old URLs to new URLs
- Store redirects in Umbraco or separate redirect table

### Migration Checklist

- [ ] Export all WordPress posts with metadata
- [ ] Download all media files from WordPress
- [ ] Create content types in Umbraco (blogPost, author, category, tag)
- [ ] Import authors first (reference by ID)
- [ ] Import categories and tags
- [ ] Import posts with proper relationships
- [ ] Upload media to Umbraco Media Library
- [ ] Test content rendering
- [ ] Create URL redirect rules
- [ ] Set up analytics tracking
- [ ] Configure caching strategy

## Deployment

### Production Requirements

- .NET 9 Runtime
- SQL Server 2019+ or Azure SQL Database
- IIS 10+ or Azure App Service
- HTTPS certificate (Let's Encrypt or commercial)

### Environment Variables

Set in production `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "umbracoDbDSN": "Server=...;Database=...;User Id=...;Password=..."
  },
  "SiteSettings": {
    "Url": "https://thesiliconpost.com"
  },
  "Umbraco": {
    "CMS": {
      "Hosting": {
        "Debug": false
      }
    }
  }
}
```

### Deployment Steps

1. Publish application: `dotnet publish -c Release`
2. Upload to server or Azure App Service
3. Configure connection strings
4. Run database migrations (Umbraco handles automatically)
5. Set up SSL certificate
6. Configure CDN for static assets (optional)
7. Enable output caching for better performance

### Performance Optimization

- Enable output caching for blog posts
- Use Azure CDN or Cloudflare for static assets
- Configure SQL Server performance (indexes, execution plans)
- Implement `IMemoryCache` for frequently accessed data
- Consider Redis for distributed caching in multi-server environments
