using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models;

namespace TheSiliconPost.Models
{
    public class Category : PublishedContentWrapped
    {
        public Category(IPublishedContent content, IPublishedValueFallback publishedValueFallback) 
            : base(content, publishedValueFallback)
        {
        }

        public new string Name => this.Value<string>("name") ?? base.Name;
        public string? Description => this.Value<string>("description");
        public string? Slug => this.UrlSegment;
        public string Url => this.Url();
    }
}
