using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;

namespace TheSiliconPost.Services
{
    /// <summary>
    /// Service to automatically create all required Document Types and Templates
    /// </summary>
    public class UmbracoSetupService
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IFileService _fileService;
        private readonly IDataTypeService _dataTypeService;
        private readonly IShortStringHelper _shortStringHelper;

        public UmbracoSetupService(
            IContentTypeService contentTypeService,
            IFileService fileService,
            IDataTypeService dataTypeService,
            IShortStringHelper shortStringHelper)
        {
            _contentTypeService = contentTypeService;
            _fileService = fileService;
            _dataTypeService = dataTypeService;
            _shortStringHelper = shortStringHelper;
        }

        /// <summary>
        /// Creates all required Document Types for The Silicon Post
        /// </summary>
        public void CreateDocumentTypes()
        {
            // Create in order of dependency
            CreateAuthorDocumentType();
            CreateCategoryDocumentType();
            CreateTagDocumentType();
            CreateHomeDocumentType();
            CreateBlogPostDocumentType();
        }

        private void CreateAuthorDocumentType()
        {
            const string alias = "author";
            
            // Check if already exists
            if (_contentTypeService.Get(alias) != null)
            {
                Console.WriteLine($"Document Type '{alias}' already exists. Skipping.");
                return;
            }

            var contentType = new ContentType(_shortStringHelper, -1)
            {
                Name = "Author",
                Alias = alias,
                Icon = "icon-user",
                Description = "Author profile with bio and social links",
                AllowedAsRoot = false
            };

            // Get data type keys
            var textstringDataType = _dataTypeService.GetDataType("Textstring");
            var textareaDataType = _dataTypeService.GetDataType("Textarea");
            var richtextDataType = _dataTypeService.GetDataType("Richtext Editor");
            var mediaPickerDataType = _dataTypeService.GetDataType("Media Picker");

            // Add properties
            var nameProperty = new PropertyType(_shortStringHelper, textstringDataType!, "name")
            {
                Name = "Name",
                Description = "Author's full name",
                Mandatory = true,
                SortOrder = 0
            };
            contentType.AddPropertyType(nameProperty, "Content");

            var bioProperty = new PropertyType(_shortStringHelper, richtextDataType!, "bio")
            {
                Name = "Bio",
                Description = "Author biography",
                Mandatory = false,
                SortOrder = 1
            };
            contentType.AddPropertyType(bioProperty, "Content");

            var avatarProperty = new PropertyType(_shortStringHelper, mediaPickerDataType!, "avatar")
            {
                Name = "Avatar",
                Description = "Author profile picture",
                Mandatory = false,
                SortOrder = 2
            };
            contentType.AddPropertyType(avatarProperty, "Content");

            var twitterProperty = new PropertyType(_shortStringHelper, textstringDataType!, "twitterUrl")
            {
                Name = "Twitter URL",
                Description = "Twitter profile URL",
                Mandatory = false,
                SortOrder = 3
            };
            contentType.AddPropertyType(twitterProperty, "Social Media");

            var githubProperty = new PropertyType(_shortStringHelper, textstringDataType!, "githubUrl")
            {
                Name = "GitHub URL",
                Description = "GitHub profile URL",
                Mandatory = false,
                SortOrder = 4
            };
            contentType.AddPropertyType(githubProperty, "Social Media");

            var linkedinProperty = new PropertyType(_shortStringHelper, textstringDataType!, "linkedInUrl")
            {
                Name = "LinkedIn URL",
                Description = "LinkedIn profile URL",
                Mandatory = false,
                SortOrder = 5
            };
            contentType.AddPropertyType(linkedinProperty, "Social Media");

            // Assign template
            var template = _fileService.GetTemplate("Author");
            if (template != null)
            {
                contentType.AllowedTemplates = new[] { template };
                contentType.SetDefaultTemplate(template);
            }

            _contentTypeService.Save(contentType);
            Console.WriteLine($"✅ Created Document Type: {contentType.Name}");
        }

        private void CreateCategoryDocumentType()
        {
            const string alias = "category";
            
            if (_contentTypeService.Get(alias) != null)
            {
                Console.WriteLine($"Document Type '{alias}' already exists. Skipping.");
                return;
            }

            var contentType = new ContentType(_shortStringHelper, -1)
            {
                Name = "Category",
                Alias = alias,
                Icon = "icon-folder",
                Description = "Blog post category",
                AllowedAsRoot = false
            };

            var textstringDataType = _dataTypeService.GetDataType("Textstring");
            var textareaDataType = _dataTypeService.GetDataType("Textarea");

            var nameProperty = new PropertyType(_shortStringHelper, textstringDataType!, "name")
            {
                Name = "Name",
                Description = "Category name",
                Mandatory = true,
                SortOrder = 0
            };
            contentType.AddPropertyType(nameProperty, "Content");

            var descriptionProperty = new PropertyType(_shortStringHelper, textareaDataType!, "description")
            {
                Name = "Description",
                Description = "Category description",
                Mandatory = false,
                SortOrder = 1
            };
            contentType.AddPropertyType(descriptionProperty, "Content");

            var slugProperty = new PropertyType(_shortStringHelper, textstringDataType!, "slug")
            {
                Name = "Slug",
                Description = "URL-friendly slug",
                Mandatory = false,
                SortOrder = 2
            };
            contentType.AddPropertyType(slugProperty, "Content");

            var template = _fileService.GetTemplate("Category");
            if (template != null)
            {
                contentType.AllowedTemplates = new[] { template };
                contentType.SetDefaultTemplate(template);
            }

            _contentTypeService.Save(contentType);
            Console.WriteLine($"✅ Created Document Type: {contentType.Name}");
        }

        private void CreateTagDocumentType()
        {
            const string alias = "tag";
            
            if (_contentTypeService.Get(alias) != null)
            {
                Console.WriteLine($"Document Type '{alias}' already exists. Skipping.");
                return;
            }

            var contentType = new ContentType(_shortStringHelper, -1)
            {
                Name = "Tag",
                Alias = alias,
                Icon = "icon-tag",
                Description = "Blog post tag",
                AllowedAsRoot = false
            };

            var textstringDataType = _dataTypeService.GetDataType("Textstring");

            var nameProperty = new PropertyType(_shortStringHelper, textstringDataType!, "name")
            {
                Name = "Name",
                Description = "Tag name",
                Mandatory = true,
                SortOrder = 0
            };
            contentType.AddPropertyType(nameProperty, "Content");

            var slugProperty = new PropertyType(_shortStringHelper, textstringDataType!, "slug")
            {
                Name = "Slug",
                Description = "URL-friendly slug",
                Mandatory = false,
                SortOrder = 1
            };
            contentType.AddPropertyType(slugProperty, "Content");

            var template = _fileService.GetTemplate("Tag");
            if (template != null)
            {
                contentType.AllowedTemplates = new[] { template };
                contentType.SetDefaultTemplate(template);
            }

            _contentTypeService.Save(contentType);
            Console.WriteLine($"✅ Created Document Type: {contentType.Name}");
        }

        private void CreateHomeDocumentType()
        {
            const string alias = "home";
            
            if (_contentTypeService.Get(alias) != null)
            {
                Console.WriteLine($"Document Type '{alias}' already exists. Skipping.");
                return;
            }

            var contentType = new ContentType(_shortStringHelper, -1)
            {
                Name = "Home",
                Alias = alias,
                Icon = "icon-home",
                Description = "Homepage",
                AllowedAsRoot = true
            };

            var textstringDataType = _dataTypeService.GetDataType("Textstring");
            var textareaDataType = _dataTypeService.GetDataType("Textarea");

            var titleProperty = new PropertyType(_shortStringHelper, textstringDataType!, "title")
            {
                Name = "Title",
                Description = "Site title",
                Mandatory = true,
                SortOrder = 0
            };
            contentType.AddPropertyType(titleProperty, "Content");

            var metaProperty = new PropertyType(_shortStringHelper, textareaDataType!, "metaDescription")
            {
                Name = "Meta Description",
                Description = "SEO meta description",
                Mandatory = false,
                SortOrder = 1
            };
            contentType.AddPropertyType(metaProperty, "Content");

            var template = _fileService.GetTemplate("Home");
            if (template != null)
            {
                contentType.AllowedTemplates = new[] { template };
                contentType.SetDefaultTemplate(template);
            }

            // Allow child node types
            var author = _contentTypeService.Get("author");
            var category = _contentTypeService.Get("category");
            var tag = _contentTypeService.Get("tag");
            var blogPost = _contentTypeService.Get("blogPost"); // Will be null first time, added later

            var allowedTypes = new List<ContentTypeSort>();
            if (author != null) allowedTypes.Add(new ContentTypeSort(author.Key, 0, author.Alias));
            if (category != null) allowedTypes.Add(new ContentTypeSort(category.Key, 1, category.Alias));
            if (tag != null) allowedTypes.Add(new ContentTypeSort(tag.Key, 2, tag.Alias));

            contentType.AllowedContentTypes = allowedTypes;

            _contentTypeService.Save(contentType);
            Console.WriteLine($"✅ Created Document Type: {contentType.Name}");
        }

        private void CreateBlogPostDocumentType()
        {
            const string alias = "blogPost";
            
            if (_contentTypeService.Get(alias) != null)
            {
                Console.WriteLine($"Document Type '{alias}' already exists. Skipping.");
                return;
            }

            var contentType = new ContentType(_shortStringHelper, -1)
            {
                Name = "Blog Post",
                Alias = alias,
                Icon = "icon-document",
                Description = "Blog post article",
                AllowedAsRoot = false
            };

            // Get data types
            var textstringDataType = _dataTypeService.GetDataType("Textstring");
            var textareaDataType = _dataTypeService.GetDataType("Textarea");
            var richtextDataType = _dataTypeService.GetDataType("Richtext Editor");
            var mediaPickerDataType = _dataTypeService.GetDataType("Media Picker");
            var datePickerDataType = _dataTypeService.GetDataType("Date Picker");
            var contentPickerDataType = _dataTypeService.GetDataType("Content Picker");

            // Title
            var titleProperty = new PropertyType(_shortStringHelper, textstringDataType!, "title")
            {
                Name = "Title",
                Description = "Blog post title",
                Mandatory = true,
                SortOrder = 0
            };
            contentType.AddPropertyType(titleProperty, "Content");

            // Content
            var contentProperty = new PropertyType(_shortStringHelper, richtextDataType!, "content")
            {
                Name = "Content",
                Description = "Main blog post content",
                Mandatory = true,
                SortOrder = 1
            };
            contentType.AddPropertyType(contentProperty, "Content");

            // Excerpt
            var excerptProperty = new PropertyType(_shortStringHelper, textareaDataType!, "excerpt")
            {
                Name = "Excerpt",
                Description = "Short summary (160 characters)",
                Mandatory = true,
                SortOrder = 2
            };
            contentType.AddPropertyType(excerptProperty, "Content");

            // Featured Image
            var featuredImageProperty = new PropertyType(_shortStringHelper, mediaPickerDataType!, "featuredImage")
            {
                Name = "Featured Image",
                Description = "Main post image",
                Mandatory = true,
                SortOrder = 3
            };
            contentType.AddPropertyType(featuredImageProperty, "Content");

            // Publish Date
            var publishDateProperty = new PropertyType(_shortStringHelper, datePickerDataType!, "publishDate")
            {
                Name = "Publish Date",
                Description = "Publication date",
                Mandatory = true,
                SortOrder = 4
            };
            contentType.AddPropertyType(publishDateProperty, "Content");

            // Last Updated
            var lastUpdatedProperty = new PropertyType(_shortStringHelper, datePickerDataType!, "lastUpdated")
            {
                Name = "Last Updated",
                Description = "Last modified date",
                Mandatory = false,
                SortOrder = 5
            };
            contentType.AddPropertyType(lastUpdatedProperty, "Content");

            // Author
            var authorProperty = new PropertyType(_shortStringHelper, contentPickerDataType!, "author")
            {
                Name = "Author",
                Description = "Post author",
                Mandatory = true,
                SortOrder = 0
            };
            contentType.AddPropertyType(authorProperty, "Metadata");

            // Categories
            var categoriesProperty = new PropertyType(_shortStringHelper, contentPickerDataType!, "categories")
            {
                Name = "Categories",
                Description = "Post categories",
                Mandatory = false,
                SortOrder = 1
            };
            contentType.AddPropertyType(categoriesProperty, "Metadata");

            // Tags
            var tagsProperty = new PropertyType(_shortStringHelper, contentPickerDataType!, "tags")
            {
                Name = "Tags",
                Description = "Post tags",
                Mandatory = false,
                SortOrder = 2
            };
            contentType.AddPropertyType(tagsProperty, "Metadata");

            // Meta Description
            var metaProperty = new PropertyType(_shortStringHelper, textareaDataType!, "metaDescription")
            {
                Name = "Meta Description",
                Description = "SEO meta description",
                Mandatory = false,
                SortOrder = 0
            };
            contentType.AddPropertyType(metaProperty, "SEO");

            var template = _fileService.GetTemplate("BlogPost");
            if (template != null)
            {
                contentType.AllowedTemplates = new[] { template };
                contentType.SetDefaultTemplate(template);
            }

            _contentTypeService.Save(contentType);
            Console.WriteLine($"✅ Created Document Type: {contentType.Name}");

            // Now update Home to allow BlogPost as child
            UpdateHomeToAllowBlogPost();
        }

        private void UpdateHomeToAllowBlogPost()
        {
            var home = _contentTypeService.Get("home");
            var blogPost = _contentTypeService.Get("blogPost");

            if (home != null && blogPost != null)
            {
                var allowedTypes = home.AllowedContentTypes?.ToList() ?? new List<ContentTypeSort>();
                
                // Check if blogPost is already allowed
                if (!allowedTypes.Any(x => x.Key == blogPost.Key))
                {
                    allowedTypes.Add(new ContentTypeSort(blogPost.Key, allowedTypes.Count, blogPost.Alias));
                    home.AllowedContentTypes = allowedTypes;
                    _contentTypeService.Save(home);
                    Console.WriteLine($"✅ Updated Home to allow BlogPost as child");
                }
            }
        }

        /// <summary>
        /// Creates a sample Home page with some initial content
        /// </summary>
        public IContent? CreateSampleHomePage(IContentService contentService)
        {
            var homeDocType = _contentTypeService.Get("home");
            if (homeDocType == null)
            {
                Console.WriteLine("❌ Home document type not found. Run CreateDocumentTypes() first.");
                return null;
            }

            // Check if home already exists
            var existingHome = contentService.GetRootContent()?.FirstOrDefault(x => x.ContentType.Alias == "home");
            if (existingHome != null)
            {
                Console.WriteLine($"Home page already exists: {existingHome.Name}");
                return existingHome;
            }

            var home = contentService.Create("The Silicon Post", -1, "home");
            home.SetValue("title", "The Silicon Post");
            home.SetValue("metaDescription", "Tech blog covering AI, Cloud, .NET, and modern software development");
            
            contentService.Save(home);
            contentService.Publish(home, Array.Empty<string>());
            Console.WriteLine($"✅ Created Home page: {home.Name}");
            
            return home;
        }
    }
}
