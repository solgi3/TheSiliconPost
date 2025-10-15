using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models;

namespace TheSiliconPost.Models
{
    public class Author : PublishedContentWrapped
    {
        public Author(IPublishedContent content, IPublishedValueFallback publishedValueFallback) 
            : base(content, publishedValueFallback)
        {
        }

        public string Name => this.Value<string>("name") ?? this.Name;
        public string Bio => this.Value<string>("bio");
        public string Email => this.Value<string>("email");
        public string Twitter => this.Value<string>("twitter");
        public string GitHub => this.Value<string>("github");
        public string LinkedIn => this.Value<string>("linkedin");
        public IPublishedContent Avatar => this.Value<IPublishedContent>("avatar");
        public string Url => this.Url();
    }
}
