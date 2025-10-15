using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models;

namespace TheSiliconPost.Models
{
    public class Tag : PublishedContentWrapped
    {
        public Tag(IPublishedContent content, IPublishedValueFallback publishedValueFallback) 
            : base(content, publishedValueFallback)
        {
        }

        public string Name => this.Value<string>("name") ?? this.Name;
        public string Slug => this.UrlSegment;
        public string Url => this.Url();
    }
}
