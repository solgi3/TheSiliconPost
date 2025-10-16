using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using TheSiliconPost.Services;

namespace TheSiliconPost.Controllers
{
    /// <summary>
    /// Controller to trigger automated Umbraco setup
    /// Access via: /setup/run
    /// </summary>
    public class SetupController : SurfaceController
    {
        private readonly UmbracoSetupService _setupService;
        private readonly IContentService _contentService;

        public SetupController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            UmbracoSetupService setupService,
            IContentService contentService)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _setupService = setupService;
            _contentService = contentService;
        }

        /// <summary>
        /// Run the automated setup to create all Document Types
        /// </summary>
        [Route("setup/run")]
        public IActionResult Run()
        {
            try
            {
                var results = new List<string>();
                
                results.Add("🚀 Starting Umbraco Setup...\n");
                
                // Create Document Types
                results.Add("Creating Document Types...");
                _setupService.CreateDocumentTypes();
                results.Add("✅ Document Types created successfully!\n");
                
                // Create sample home page
                results.Add("Creating sample Home page...");
                var home = _setupService.CreateSampleHomePage(_contentService);
                if (home != null)
                {
                    results.Add($"✅ Home page created: {home.Name}\n");
                }
                
                results.Add("🎉 Setup Complete!");
                results.Add("\nNext Steps:");
                results.Add("1. Go to Umbraco backoffice: /umbraco");
                results.Add("2. Navigate to Settings > Document Types to verify");
                results.Add("3. Go to Content to see your Home page");
                results.Add("4. Create your first blog post!");
                
                return Content(string.Join("\n", results), "text/plain");
            }
            catch (Exception ex)
            {
                return Content($"❌ Setup failed: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "text/plain");
            }
        }

        /// <summary>
        /// Check setup status
        /// </summary>
        [Route("setup/status")]
        public IActionResult Status()
        {
            var status = new List<string>();
            status.Add("📋 Umbraco Setup Status\n");
            
            var contentTypeService = Services.ContentTypeService;
            
            var docTypes = new[] { "home", "blogPost", "author", "category", "tag" };
            
            foreach (var alias in docTypes)
            {
                var exists = contentTypeService?.Get(alias) != null;
                var icon = exists ? "✅" : "❌";
                status.Add($"{icon} {alias}: {(exists ? "Created" : "Missing")}");
            }
            
            status.Add("\n");
            
            var homeContent = _contentService.GetRootContent()?.FirstOrDefault(x => x.ContentType.Alias == "home");
            if (homeContent != null)
            {
                status.Add($"✅ Home page exists: {homeContent.Name}");
            }
            else
            {
                status.Add("❌ Home page not created yet");
            }
            
            status.Add("\n");
            status.Add("To run setup: /setup/run");
            
            return Content(string.Join("\n", status), "text/plain");
        }
    }
}
