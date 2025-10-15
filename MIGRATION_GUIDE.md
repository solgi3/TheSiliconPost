# WordPress to Umbraco Migration Guide

This guide explains how to migrate content from your WordPress blog to Umbraco using the built-in migration service.

## Prerequisites

1. WordPress site with REST API enabled (default in WordPress 4.7+)
2. Umbraco site running with document types created
3. Access to WordPress admin panel

## Migration Options

### Option 1: WordPress REST API (Recommended)

The easiest method - fetches content directly from WordPress REST API.

**Steps:**

1. Verify WordPress REST API is accessible:
   - Visit: `https://your-wordpress-site.com/wp-json/wp/v2/posts`
   - You should see JSON output

2. In Umbraco, create parent folders for content:
   - Categories folder (note the ID)
   - Tags folder (note the ID)
   - Posts folder (note the ID)

3. Use the migration service (create a custom controller or use Umbraco Forms):

```csharp
// Example migration controller
public class MigrationController : SurfaceController
{
    private readonly WordPressMigrationService _migrationService;
    
    public async Task<IActionResult> MigrateFromWordPress()
    {
        var wordPressUrl = "https://your-wordpress-site.com";
        var postsParentId = 1234; // Replace with your posts folder ID
        
        // Import categories first
        var categoriesResult = await _migrationService.ImportCategoriesAsync(wordPressUrl, categoriesParentId);
        
        // Import tags
        var tagsResult = await _migrationService.ImportTagsAsync(wordPressUrl, tagsParentId);
        
        // Import posts
        var postsResult = await _migrationService.ImportFromRestApiAsync(wordPressUrl, postsParentId);
        
        return Ok(new { 
            categories = categoriesResult,
            tags = tagsResult,
            posts = postsResult 
        });
    }
}
```

### Option 2: WordPress XML Export

For sites where REST API is disabled or you have XML export files.

**Steps:**

1. Export from WordPress:
   - Go to **Tools → Export**
   - Select **All content**
   - Download the XML file

2. Parse XML and import (requires custom implementation):

```csharp
// TODO: Implement XML parser
// Parse WordPress WXR format
// Create Umbraco content nodes
```

### Option 3: Direct Database Migration

For advanced users with direct database access.

**Steps:**

1. Export WordPress database tables:
   - `wp_posts`
   - `wp_postmeta`
   - `wp_terms`
   - `wp_term_relationships`

2. Create migration script to:
   - Map WordPress post types to Umbraco document types
   - Transform content
   - Import into Umbraco via Content Service

## What Gets Migrated

### ✅ Currently Supported

- Post titles
- Post content (HTML)
- Post excerpts
- Publish dates
- Modified dates
- Categories (basic)
- Tags (basic)

### 🚧 To Be Implemented

- Featured images
- Post thumbnails
- Custom fields/meta data
- Authors (user mapping)
- Comments
- Post relationships (categories/tags linking)
- Media library assets

## Post-Migration Tasks

### 1. URL Redirects

Create 301 redirects from old WordPress URLs to new Umbraco URLs:

```csharp
// Example redirect mapping
var redirects = new Dictionary<string, string>
{
    ["/2024/01/15/old-post-slug/"] = "/blog/old-post-slug",
    ["/category/tech/"] = "/categories/tech",
    // ... add more redirects
};
```

**Implementation options:**
- IContentFinder in Umbraco
- IIS URL Rewrite module
- Cloudflare page rules
- Custom middleware

### 2. Media Migration

Download all WordPress media files:

```bash
# Using wget to download WordPress uploads
wget -r -np -nH --cut-dirs=2 -R "index.html*" https://your-site.com/wp-content/uploads/
```

Then upload to Umbraco Media Library.

### 3. Content Cleanup

After migration, review and fix:

- **Images**: Update image paths to Umbraco media URLs
- **Links**: Update internal links to new URL structure
- **Formatting**: Check for broken HTML or formatting issues
- **Metadata**: Verify SEO fields are populated
- **Categories/Tags**: Link posts to imported categories/tags

### 4. Verify SEO

- Check canonical URLs
- Verify meta descriptions
- Test OpenGraph tags
- Validate sitemap.xml
- Submit new sitemap to Google Search Console

## Testing the Migration

1. **Test on staging first** - Never migrate directly to production
2. **Verify content** - Check a sample of posts for accuracy
3. **Test URLs** - Ensure redirects work correctly
4. **Check media** - Verify all images display properly
5. **Test RSS feed** - Ensure feed works at `/feed/rss`
6. **Validate sitemap** - Check `/sitemap.xml` is generated

## Troubleshooting

### REST API Not Working

**Problem:** Can't access `/wp-json/wp/v2/posts`

**Solutions:**
- Check WordPress REST API is enabled (Settings → Permalinks → Save)
- Verify no security plugins are blocking the REST API
- Check .htaccess rules aren't blocking JSON endpoints

### Content Not Importing

**Problem:** Migration runs but no content appears

**Solutions:**
- Check Umbraco document types match service aliases (`blogPost`, `category`, `tag`)
- Verify property aliases match exactly (case-sensitive)
- Check Umbraco logs for errors
- Ensure parent IDs are correct

### Images Not Displaying

**Problem:** Posts import but images are broken

**Solutions:**
- Manually download WordPress uploads folder
- Update image URLs in post content
- Use find/replace to update image domains
- Implement media import in `ImportMediaAsync` method

## Performance Considerations

For large WordPress sites (1000+ posts):

- **Batch processing**: Import in batches of 100 posts
- **Background jobs**: Use Hangfire or similar for async processing
- **Progress tracking**: Implement progress reporting
- **Error handling**: Log failures and allow resume

## Security Notes

- Never expose migration endpoints in production
- Use API authentication if WordPress requires it
- Validate and sanitize all imported content
- Review imported HTML for malicious code

## Example: Complete Migration Workflow

```bash
# 1. Verify WordPress API
curl https://old-site.com/wp-json/wp/v2/posts

# 2. Create Umbraco parent nodes via backoffice
# Note the node IDs

# 3. Run migration (via custom controller or console app)
POST /migration/import
{
  "wordPressUrl": "https://old-site.com",
  "categoriesParentId": 1100,
  "tagsParentId": 1101,
  "postsParentId": 1102
}

# 4. Download media
wget -r -np -nH --cut-dirs=2 https://old-site.com/wp-content/uploads/

# 5. Upload media to Umbraco via backoffice

# 6. Update image paths in content (SQL or programmatic)

# 7. Test and verify
```

## Need Help?

- Check WordPress REST API docs: https://developer.wordpress.org/rest-api/
- Umbraco Content Service docs: https://docs.umbraco.com/umbraco-cms/reference/management/services/contentservice
- Open an issue on GitHub for migration problems

---

**Next:** After migration, set up URL redirects and configure production deployment.
