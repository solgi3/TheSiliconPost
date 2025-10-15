using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace TheSiliconPost.Controllers
{
    [Route("sitemap.xml")]
    public class SitemapController : SurfaceController
    {
        private readonly IConfiguration _configuration;

        public SitemapController(
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
        public IActionResult Index()
        {
            var siteUrl = _configuration["SiteSettings:Url"] ?? "https://thesiliconpost.com";
            var rootNodes = UmbracoContext.Content.GetAtRoot();
            var allNodes = rootNodes.SelectMany(x => x.DescendantsOrSelf()).Where(x => !x.Value<bool>("umbracoNaviHide"));

            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            // Homepage
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{siteUrl}/</loc>");
            sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("    <changefreq>daily</changefreq>");
            sb.AppendLine("    <priority>1.0</priority>");
            sb.AppendLine("  </url>");

            foreach (var node in allNodes)
            {
                var url = $"{siteUrl}{node.Url()}";
                var lastMod = node.UpdateDate;
                var priority = node.ContentType.Alias == "blogPost" ? "0.8" : "0.5";
                var changefreq = node.ContentType.Alias == "blogPost" ? "weekly" : "monthly";

                sb.AppendLine("  <url>");
                sb.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(url)}</loc>");
                sb.AppendLine($"    <lastmod>{lastMod:yyyy-MM-dd}</lastmod>");
                sb.AppendLine($"    <changefreq>{changefreq}</changefreq>");
                sb.AppendLine($"    <priority>{priority}</priority>");
                sb.AppendLine("  </url>");
            }

            sb.AppendLine("</urlset>");

            return Content(sb.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
