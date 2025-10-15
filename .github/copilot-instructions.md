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
