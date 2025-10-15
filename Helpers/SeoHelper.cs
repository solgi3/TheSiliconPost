using TheSiliconPost.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace TheSiliconPost.Helpers
{
    public class SeoHelper
    {
        private readonly string _siteUrl;
        private readonly string _siteName;

        public SeoHelper(IConfiguration configuration)
        {
            _siteUrl = configuration["SiteSettings:Url"] ?? "https://thesiliconpost.com";
            _siteName = configuration["SiteSettings:Name"] ?? "The Silicon Post";
        }

        public string GetMetaDescription(BlogPost post)
        {
            return post.MetaDescription ?? post.Excerpt ?? StripHtml(post.Content?.ToString() ?? "").Substring(0, Math.Min(160, post.Content?.ToString()?.Length ?? 0));
        }

        public string GetCanonicalUrl(BlogPost post)
        {
            return $"{_siteUrl}{post.Url}";
        }

        public Dictionary<string, string> GetOpenGraphTags(BlogPost post)
        {
            return new Dictionary<string, string>
            {
                ["og:type"] = "article",
                ["og:title"] = post.Title,
                ["og:description"] = GetMetaDescription(post),
                ["og:url"] = GetCanonicalUrl(post),
                ["og:image"] = post.OgImage?.Url() ?? post.FeaturedImage?.Url() ?? $"{_siteUrl}/images/default-og.jpg",
                ["og:site_name"] = _siteName,
                ["article:published_time"] = post.PublishDate.ToString("o"),
                ["article:modified_time"] = (post.LastUpdated ?? post.PublishDate).ToString("o"),
                ["article:author"] = post.Author?.Name ?? _siteName,
                ["article:section"] = post.Categories?.FirstOrDefault()?.Name ?? "Technology"
            };
        }

        public Dictionary<string, string> GetTwitterCardTags(BlogPost post)
        {
            return new Dictionary<string, string>
            {
                ["twitter:card"] = "summary_large_image",
                ["twitter:title"] = post.Title,
                ["twitter:description"] = GetMetaDescription(post),
                ["twitter:image"] = post.OgImage?.Url() ?? post.FeaturedImage?.Url() ?? $"{_siteUrl}/images/default-og.jpg",
                ["twitter:site"] = "@TheSiliconPost"  // Update with your Twitter handle
            };
        }

        public string GetStructuredData(BlogPost post)
        {
            var imageUrl = post.FeaturedImage?.Url() ?? $"{_siteUrl}/images/default-og.jpg";
            
            return $@"{{
    ""@context"": ""https://schema.org"",
    ""@type"": ""BlogPosting"",
    ""headline"": ""{EscapeJson(post.Title)}"",
    ""description"": ""{EscapeJson(GetMetaDescription(post))}"",
    ""image"": ""{imageUrl}"",
    ""datePublished"": ""{post.PublishDate:o}"",
    ""dateModified"": ""{(post.LastUpdated ?? post.PublishDate):o}"",
    ""author"": {{
        ""@type"": ""Person"",
        ""name"": ""{EscapeJson(post.Author?.Name ?? _siteName)}""
    }},
    ""publisher"": {{
        ""@type"": ""Organization"",
        ""name"": ""{_siteName}"",
        ""logo"": {{
            ""@type"": ""ImageObject"",
            ""url"": ""{_siteUrl}/images/logo.png""
        }}
    }},
    ""mainEntityOfPage"": {{
        ""@type"": ""WebPage"",
        ""@id"": ""{GetCanonicalUrl(post)}""
    }},
    ""keywords"": ""{EscapeJson(post.MetaKeywords ?? string.Join(", ", post.Tags?.Select(t => t.Name) ?? Enumerable.Empty<string>()))}""
}}";
        }

        private string StripHtml(string html)
        {
            return System.Text.RegularExpressions.Regex.Replace(html ?? "", "<.*?>", string.Empty);
        }

        private string EscapeJson(string text)
        {
            return text?.Replace("\"", "\\\"").Replace("\n", " ").Replace("\r", "") ?? "";
        }
    }
}
