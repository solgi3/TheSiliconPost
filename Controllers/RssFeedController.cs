using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using TheSiliconPost.Models;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace TheSiliconPost.Controllers
{
    [Route("feed")]
    public class RssFeedController : SurfaceController
    {
        private readonly IConfiguration _configuration;

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

        [HttpGet]
        [Route("")]
        [Route("rss")]
        public IActionResult Index()
        {
            var siteUrl = _configuration["SiteSettings:Url"] ?? "https://thesiliconpost.com";
            var siteName = _configuration["SiteSettings:Name"] ?? "The Silicon Post";
            var siteDescription = _configuration["SiteSettings:Description"] ?? "Tech blog covering AI, Cloud, .NET, and modern software development";

            var feed = new SyndicationFeed(siteName, siteDescription, new Uri(siteUrl))
            {
                Language = "en-US",
                Copyright = new TextSyndicationContent($"© {DateTime.Now.Year} {siteName}"),
                LastUpdatedTime = DateTimeOffset.UtcNow
            };

            var posts = UmbracoContext.Content.GetAtRoot()
                .DescendantsOrSelfOfType("blogPost")
                .OrderByDescending(x => x.Value<DateTime>("publishDate"))
                .Take(50)
                .ToList();

            var items = new List<SyndicationItem>();

            foreach (var post in posts)
            {
                var blogPost = new BlogPost(post, null);
                
                var item = new SyndicationItem(
                    blogPost.Title,
                    blogPost.Excerpt ?? StripHtml(blogPost.Content?.ToString() ?? "").Substring(0, Math.Min(200, blogPost.Content?.ToString()?.Length ?? 0)),
                    new Uri($"{siteUrl}{blogPost.Url}"),
                    blogPost.Id.ToString(),
                    blogPost.PublishDate
                );

                if (blogPost.Categories != null)
                {
                    foreach (var category in blogPost.Categories)
                    {
                        item.Categories.Add(new SyndicationCategory(category.Name));
                    }
                }

                items.Add(item);
            }

            feed.Items = items;

            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true
            };

            using var stream = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(stream, settings))
            {
                var rssFormatter = new Rss20FeedFormatter(feed, false);
                rssFormatter.WriteTo(xmlWriter);
                xmlWriter.Flush();
            }

            return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
        }

        private string StripHtml(string html)
        {
            return System.Text.RegularExpressions.Regex.Replace(html ?? "", "<.*?>", string.Empty);
        }
    }
}
