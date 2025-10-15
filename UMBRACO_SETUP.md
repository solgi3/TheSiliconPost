# Umbraco Document Types Setup Guide

This guide explains how to create the necessary Document Types in the Umbraco backoffice to match the strongly-typed models in the codebase.

## Prerequisites

1. Run the application: `dotnet run`
2. Navigate to `https://localhost:5001/umbraco`
3. Complete the Umbraco installation wizard
4. Log in to the backoffice

## Document Types to Create

### 1. Home (Document Type)

**Alias**: `home`  
**Icon**: 🏠 (icon-home)  
**Template**: `Home.cshtml`  
**Allowed as Root**: ✅ Yes

**Properties**:

- Title (alias: `title`, type: Textstring)
- Meta Description (alias: `metaDescription`, type: Textarea)

**Allowed Child Node Types**: `blogPost`, `category`, `tag`, `author`

---

### 2. Blog Post (Document Type)

**Alias**: `blogPost`  
**Icon**: 📝 (icon-document)  
**Template**: `BlogPost.cshtml`  
**Allowed as Root**: ❌ No

**Properties**:

| Name             | Alias             | Data Type                 | Required | Description               |
| ---------------- | ----------------- | ------------------------- | -------- | ------------------------- |
| Title            | `title`           | Textstring                | ✅       | Post title                |
| Content          | `content`         | Rich Text Editor          | ✅       | Main post content         |
| Excerpt          | `excerpt`         | Textarea                  | ✅       | Short summary (160 chars) |
| Featured Image   | `featuredImage`   | Media Picker              | ✅       | Main post image           |
| Publish Date     | `publishDate`     | Date Picker               | ✅       | Publication date          |
| Last Updated     | `lastUpdated`     | Date Picker               | ❌       | Last modified date        |
| Author           | `author`          | Content Picker (author)   | ✅       | Post author               |
| Categories       | `categories`      | Content Picker (multiple) | ❌       | Post categories           |
| Tags             | `tags`            | Content Picker (multiple) | ❌       | Post tags                 |
| Meta Description | `metaDescription` | Textarea                  | ❌       | SEO meta description      |
| Meta Keywords    | `metaKeywords`    | Textstring                | ❌       | SEO keywords              |
| OG Image         | `ogImage`         | Media Picker              | ❌       | Social share image        |
| Is Featured      | `isFeatured`      | Toggle                    | ❌       | Featured post flag        |

**Allowed Child Node Types**: None

---

### 3. Author (Document Type)

**Alias**: `author`  
**Icon**: 👤 (icon-user)  
**Template**: `Author.cshtml`  
**Allowed as Root**: ❌ No

**Properties**:

| Name     | Alias      | Data Type                  | Required | Description           |
| -------- | ---------- | -------------------------- | -------- | --------------------- |
| Name     | `name`     | Textstring                 | ✅       | Author full name      |
| Bio      | `bio`      | Textarea                   | ❌       | Author biography      |
| Email    | `email`    | Textstring (Email Address) | ❌       | Contact email         |
| Avatar   | `avatar`   | Media Picker               | ❌       | Profile picture       |
| Twitter  | `twitter`  | Textstring                 | ❌       | Twitter handle (no @) |
| GitHub   | `github`   | Textstring                 | ❌       | GitHub username       |
| LinkedIn | `linkedin` | Textstring                 | ❌       | LinkedIn username     |

**Allowed Child Node Types**: None

---

### 4. Category (Document Type)

**Alias**: `category`  
**Icon**: 📁 (icon-folder)  
**Template**: `Category.cshtml`  
**Allowed as Root**: ❌ No

**Properties**:

| Name        | Alias         | Data Type  | Required | Description          |
| ----------- | ------------- | ---------- | -------- | -------------------- |
| Name        | `name`        | Textstring | ✅       | Category name        |
| Description | `description` | Textarea   | ❌       | Category description |

**Allowed Child Node Types**: None

**URL Segment**: Use category name as URL slug

---

### 5. Tag (Document Type)

**Alias**: `tag`  
**Icon**: 🏷️ (icon-tag)  
**Template**: `Tag.cshtml`  
**Allowed as Root**: ❌ No

**Properties**:

| Name | Alias  | Data Type  | Required | Description |
| ---- | ------ | ---------- | -------- | ----------- |
| Name | `name` | Textstring | ✅       | Tag name    |

**Allowed Child Node Types**: None

**URL Segment**: Use tag name as URL slug

---

## Content Structure Recommendation

```
📦 Content
 ├── 🏠 Home (home)
 │   ├── 📝 Blog Post 1 (blogPost)
 │   ├── 📝 Blog Post 2 (blogPost)
 │   ├── 📁 Categories
 │   │   ├── 📁 AI & Machine Learning (category)
 │   │   ├── 📁 Cloud Computing (category)
 │   │   └── 📁 .NET Development (category)
 │   ├── 🏷️ Tags
 │   │   ├── 🏷️ C# (tag)
 │   │   ├── 🏷️ Azure (tag)
 │   │   └── 🏷️ Umbraco (tag)
 │   └── 👤 Authors
 │       ├── 👤 Mohammad Yosef Solgi (author)
 │       └── 👤 Guest Author (author)
```

## Step-by-Step Setup Instructions

### Step 1: Create Document Types

1. Go to **Settings** → **Document Types**
2. Click **Create** → **Document Type**
3. For each document type above:
   - Set the **Name** and **Alias**
   - Choose an **Icon**
   - Add all properties with correct aliases and data types
   - Configure **Structure** settings (allowed child node types, allowed at root)
   - Assign the **Template** (must match View filename)

### Step 2: Create Templates

Templates should already exist in `Views/` folder:

- ✅ `_Layout.cshtml`
- ✅ `Home.cshtml`
- ✅ `BlogPost.cshtml`
- ✅ `Author.cshtml`
- ✅ `Category.cshtml`
- ✅ `Tag.cshtml`

If templates don't appear in Umbraco, sync them:

1. Go to **Settings** → **Templates**
2. Click **Reload** or restart the application

### Step 3: Create Content

1. Go to **Content** section
2. Create the **Home** page (root node)
3. Under Home, create folder nodes:
   - Categories (optional container)
   - Tags (optional container)
   - Authors (optional container)
4. Create your first **Author**
5. Create some **Categories** and **Tags**
6. Create your first **Blog Post**
   - Fill in all required fields
   - Link to author, categories, and tags
   - Upload a featured image

### Step 4: Configure Media Types

1. Go to **Media** section
2. Upload images for:
   - Blog post featured images
   - Author avatars
   - OG images for social sharing

### Step 5: Test the Site

1. Save and publish all content
2. Navigate to the frontend: `https://localhost:5001`
3. Test URLs:
   - Home: `/`
   - Blog post: `/blog-post-slug`
   - Category: `/categories/category-name`
   - Tag: `/tags/tag-name`
   - Author: `/authors/author-name`
   - RSS: `/feed/rss`
   - Sitemap: `/sitemap.xml`

## Important Notes

- **Property Aliases**: Must match exactly with the C# models (camelCase)
- **View Templates**: Template names must match the `.cshtml` filenames
- **Document Type Aliases**: Must match exactly with code (e.g., `DescendantsOrSelfOfType("blogPost")`)
- **Content Pickers**: Configure to only allow specific document types (e.g., author picker only shows authors)

## Troubleshooting

**Content not appearing?**

- Ensure content is **Published** (not just saved)
- Check that the correct template is assigned
- Verify property aliases match the C# models

**Template not found?**

- Check that `.cshtml` files are in the `Views` folder
- Restart the application
- Sync templates in Umbraco backoffice

**SEO metadata not working?**

- Verify `SiteSettings` in `appsettings.json`
- Check that `SeoHelper` is properly injected in views
- Ensure meta properties have values in content

## Next Steps

After setting up document types and creating content:

1. Migrate existing WordPress content (see migration guide)
2. Set up URL redirects from old WordPress URLs
3. Configure production environment
4. Deploy to hosting

---

For more information, see [Umbraco Documentation](https://docs.umbraco.com/)
