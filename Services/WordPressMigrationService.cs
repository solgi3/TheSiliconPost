using System.Text.Json;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Models;

namespace TheSiliconPost.Services
{
    /// <summary>
    /// Service for migrating content from WordPress to Umbraco
    /// Supports WordPress REST API and XML export formats
    /// </summary>
    public class WordPressMigrationService
    {
        private readonly IContentService _contentService;
        private readonly IMediaService _mediaService;
        private readonly ILogger<WordPressMigrationService> _logger;
        private readonly HttpClient _httpClient;

        public WordPressMigrationService(
            IContentService contentService,
            IMediaService mediaService,
            ILogger<WordPressMigrationService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _contentService = contentService;
            _mediaService = mediaService;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Import posts from WordPress REST API
        /// </summary>
        public async Task<MigrationResult> ImportFromRestApiAsync(string wordPressUrl, int parentId)
        {
            var result = new MigrationResult();
            
            try
            {
                // Fetch posts from WordPress REST API
                var postsUrl = $"{wordPressUrl}/wp-json/wp/v2/posts?per_page=100";
                var response = await _httpClient.GetStringAsync(postsUrl);
                var posts = JsonSerializer.Deserialize<List<WordPressPost>>(response);

                if (posts == null)
                {
                    result.Errors.Add("Failed to deserialize posts from WordPress API");
                    return result;
                }

                foreach (var post in posts)
                {
                    try
                    {
                        await ImportPostAsync(post, parentId);
                        result.ImportedCount++;
                        _logger.LogInformation($"Imported post: {post.Title?.Rendered}");
                    }
                    catch (Exception ex)
                    {
                        result.FailedCount++;
                        result.Errors.Add($"Failed to import '{post.Title?.Rendered}': {ex.Message}");
                        _logger.LogError(ex, $"Error importing post: {post.Title?.Rendered}");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Failed to fetch posts from WordPress: {ex.Message}");
                _logger.LogError(ex, "Error fetching posts from WordPress API");
            }

            return result;
        }

        /// <summary>
        /// Import a single WordPress post
        /// </summary>
        private async Task ImportPostAsync(WordPressPost wpPost, int parentId)
        {
            var content = _contentService.Create(
                wpPost.Title?.Rendered ?? "Untitled",
                parentId,
                "blogPost"
            );

            // Set basic properties
            content.SetValue("title", wpPost.Title?.Rendered ?? "Untitled");
            content.SetValue("content", wpPost.Content?.Rendered ?? "");
            content.SetValue("excerpt", wpPost.Excerpt?.Rendered ?? "");
            
            // Parse and set publish date
            if (DateTime.TryParse(wpPost.Date, out var publishDate))
            {
                content.SetValue("publishDate", publishDate);
            }

            // Set modified date
            if (DateTime.TryParse(wpPost.Modified, out var modifiedDate))
            {
                content.SetValue("lastUpdated", modifiedDate);
            }

            // TODO: Import featured image
            // TODO: Import categories and tags
            // TODO: Import author

            _contentService.Save(content);
            _contentService.Publish(content, Array.Empty<string>());
        }

        /// <summary>
        /// Import categories from WordPress
        /// </summary>
        public async Task<MigrationResult> ImportCategoriesAsync(string wordPressUrl, int parentId)
        {
            var result = new MigrationResult();
            
            try
            {
                var categoriesUrl = $"{wordPressUrl}/wp-json/wp/v2/categories?per_page=100";
                var response = await _httpClient.GetStringAsync(categoriesUrl);
                var categories = JsonSerializer.Deserialize<List<WordPressCategory>>(response);

                if (categories == null)
                {
                    result.Errors.Add("Failed to deserialize categories from WordPress API");
                    return result;
                }

                foreach (var category in categories)
                {
                    try
                    {
                        var content = _contentService.Create(
                            category.Name ?? "Untitled Category",
                            parentId,
                            "category"
                        );

                        content.SetValue("name", category.Name ?? "");
                        content.SetValue("description", category.Description ?? "");

                        _contentService.Save(content);
                        _contentService.Publish(content, Array.Empty<string>());
                        result.ImportedCount++;
                        _logger.LogInformation($"Imported category: {category.Name}");
                    }
                    catch (Exception ex)
                    {
                        result.FailedCount++;
                        result.Errors.Add($"Failed to import category '{category.Name}': {ex.Message}");
                        _logger.LogError(ex, $"Error importing category: {category.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Failed to fetch categories: {ex.Message}");
                _logger.LogError(ex, "Error fetching categories from WordPress API");
            }

            return result;
        }

        /// <summary>
        /// Import tags from WordPress
        /// </summary>
        public async Task<MigrationResult> ImportTagsAsync(string wordPressUrl, int parentId)
        {
            var result = new MigrationResult();
            
            try
            {
                var tagsUrl = $"{wordPressUrl}/wp-json/wp/v2/tags?per_page=100";
                var response = await _httpClient.GetStringAsync(tagsUrl);
                var tags = JsonSerializer.Deserialize<List<WordPressTag>>(response);

                if (tags == null)
                {
                    result.Errors.Add("Failed to deserialize tags from WordPress API");
                    return result;
                }

                foreach (var tag in tags)
                {
                    try
                    {
                        var content = _contentService.Create(
                            tag.Name ?? "Untitled Tag",
                            parentId,
                            "tag"
                        );

                        content.SetValue("name", tag.Name ?? "");

                        _contentService.Save(content);
                        _contentService.Publish(content, Array.Empty<string>());
                        result.ImportedCount++;
                        _logger.LogInformation($"Imported tag: {tag.Name}");
                    }
                    catch (Exception ex)
                    {
                        result.FailedCount++;
                        result.Errors.Add($"Failed to import tag '{tag.Name}': {ex.Message}");
                        _logger.LogError(ex, $"Error importing tag: {tag.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Failed to fetch tags: {ex.Message}");
                _logger.LogError(ex, "Error fetching tags from WordPress API");
            }

            return result;
        }

        /// <summary>
        /// Download and import media from WordPress
        /// </summary>
        public async Task<int?> ImportMediaAsync(string mediaUrl, int parentId = -1)
        {
            try
            {
                var response = await _httpClient.GetAsync(mediaUrl);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to download media: {mediaUrl}");
                    return null;
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var fileName = Path.GetFileName(new Uri(mediaUrl).LocalPath);
                
                var media = _mediaService.CreateMedia(fileName, parentId, "Image");
                // TODO: Implement proper media file upload using Umbraco's media file system
                // media.SetValue("umbracoFile", fileName);
                
                _mediaService.Save(media);
                
                return media.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error importing media: {mediaUrl}");
                return null;
            }
        }
    }

    #region WordPress Models

    public class WordPressPost
    {
        public int Id { get; set; }
        public WordPressRendered? Title { get; set; }
        public WordPressRendered? Content { get; set; }
        public WordPressRendered? Excerpt { get; set; }
        public string? Date { get; set; }
        public string? Modified { get; set; }
        public string? Slug { get; set; }
        public string? Status { get; set; }
        public int Author { get; set; }
        public int FeaturedMedia { get; set; }
        public List<int>? Categories { get; set; }
        public List<int>? Tags { get; set; }
    }

    public class WordPressCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
    }

    public class WordPressTag
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
    }

    public class WordPressRendered
    {
        public string? Rendered { get; set; }
    }

    #endregion

    #region Migration Result

    public class MigrationResult
    {
        public int ImportedCount { get; set; }
        public int FailedCount { get; set; }
        public List<string> Errors { get; set; } = new();
        public bool IsSuccess => FailedCount == 0 && Errors.Count == 0;
    }

    #endregion
}
