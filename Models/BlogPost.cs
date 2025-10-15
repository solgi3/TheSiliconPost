using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models;

namespace TheSiliconPost.Models
{
    /// <summary>
    /// Blog Post document type for The Silicon Post
    /// </summary>
    public class BlogPost : PublishedContentWrapped
    {
        public BlogPost(IPublishedContent content, IPublishedValueFallback publishedValueFallback) 
            : base(content, publishedValueFallback)
        {
        }

        public string Title => this.Value<string>("title") ?? this.Name;
        public IHtmlEncodedString Content => this.Value<IHtmlEncodedString>("content");
        public string Excerpt => this.Value<string>("excerpt");
        public IPublishedContent FeaturedImage => this.Value<IPublishedContent>("featuredImage");
        public DateTime PublishDate => this.Value<DateTime>("publishDate");
        public DateTime? LastUpdated => this.Value<DateTime?>("lastUpdated");
        
        public IEnumerable<Category> Categories => this.Value<IEnumerable<IPublishedContent>>("categories")
            ?.Select(x => new Category(x, PublishedValueFallback)) ?? Enumerable.Empty<Category>();
        
        public IEnumerable<Tag> Tags => this.Value<IEnumerable<IPublishedContent>>("tags")
            ?.Select(x => new Tag(x, PublishedValueFallback)) ?? Enumerable.Empty<Tag>();
        
        public Author? Author
        {
            get
            {
                var authorContent = this.Value<IPublishedContent>("author");
                return authorContent != null ? new Author(authorContent, PublishedValueFallback) : null;
            }
        }
        
        public string MetaDescription => this.Value<string>("metaDescription") ?? Excerpt;
        public string MetaKeywords => this.Value<string>("metaKeywords");
        public IPublishedContent OgImage => this.Value<IPublishedContent>("ogImage") ?? FeaturedImage;
        public bool IsFeatured => this.Value<bool>("isFeatured");
        public string Slug => this.UrlSegment;
        public string Url => this.Url();
    }
}
