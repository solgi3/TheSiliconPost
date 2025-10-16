# Automated Umbraco Setup Guide

## Overview

This project includes an automated setup service that creates all required Document Types programmatically, saving you from manual configuration in the Umbraco backoffice.

## Quick Start

### Step 1: Run the Application

```bash
dotnet run
```

The application will start on:
- **HTTPS**: https://localhost:44364
- **HTTP**: http://localhost:1549

### Step 2: Complete Umbraco Installation

1. Open your browser and navigate to: **https://localhost:44364/umbraco**
2. You'll see the Umbraco installation wizard
3. Fill in the required information:
   - **Your name**: Your full name
   - **Email**: Your email address (will be your login username)
   - **Password**: Create a strong password
4. **Database**: Leave as "Microsoft SQL Server Local DB" (default)
5. Click **Install** and wait for the setup to complete
6. Click **Finish** and log in to the backoffice

### Step 3: Run the Automated Setup

Once you're logged into the Umbraco backoffice, open a new browser tab and navigate to:

**https://localhost:44364/setup/run**

You'll see output like this:

```
🚀 Starting Umbraco Setup...

Creating Document Types...
✅ Created Document Type: Author
✅ Created Document Type: Category
✅ Created Document Type: Tag
✅ Created Document Type: Home
✅ Created Document Type: Blog Post
✅ Updated Home to allow BlogPost as child
✅ Document Types created successfully!

Creating sample Home page...
✅ Home page created: The Silicon Post
✅ Home page created: The Silicon Post

🎉 Setup Complete!

Next Steps:
1. Go to Umbraco backoffice: /umbraco
2. Navigate to Settings > Document Types to verify
3. Go to Content to see your Home page
4. Create your first blog post!
```

### Step 4: Verify the Setup

1. Go back to the Umbraco backoffice
2. Click **Settings** (gear icon) in the left sidebar
3. Expand **Document Types** - you should see:
   - ✅ Author
   - ✅ Blog Post
   - ✅ Category
   - ✅ Home
   - ✅ Tag
4. Click **Content** (document icon) in the left sidebar
5. You should see your "The Silicon Post" home page

### Step 5: Create Your First Content

#### Create an Author Profile

1. In the **Content** section, right-click on "The Silicon Post"
2. Select **Create** → **Author**
3. Fill in the fields:
   - **Name**: Your name
   - **Bio**: Short biography
   - **Avatar**: Upload your profile picture
   - **Twitter URL**: https://twitter.com/yourusername (optional)
   - **GitHub URL**: https://github.com/yourusername (optional)
   - **LinkedIn URL**: https://www.linkedin.com/in/yourprofile (optional)
4. Click **Save and Publish**

#### Create Categories

1. Right-click on "The Silicon Post" → **Create** → **Category**
2. Create a few categories like:
   - **AI & Machine Learning**
   - **.NET Development**
   - **Cloud Computing**
   - **Web Development**
3. For each category:
   - **Name**: Category name
   - **Description**: Brief description
   - **Slug**: url-friendly-name (e.g., "ai-machine-learning")
4. Click **Save and Publish**

#### Create Tags

1. Right-click on "The Silicon Post" → **Create** → **Tag**
2. Create some tags like:
   - C#
   - Azure
   - ASP.NET Core
   - Umbraco
   - Docker
3. For each tag:
   - **Name**: Tag name
   - **Slug**: lowercase-name (e.g., "aspnet-core")
4. Click **Save and Publish**

#### Create Your First Blog Post

1. Right-click on "The Silicon Post" → **Create** → **Blog Post**
2. Fill in all the required fields:
   - **Title**: Your blog post title
   - **Content**: Write your article (use the rich text editor)
   - **Excerpt**: Short summary (160 characters max)
   - **Featured Image**: Upload an image
   - **Publish Date**: Select today's date
   - **Author**: Select the author you created
   - **Categories**: Select one or more categories
   - **Tags**: Select relevant tags
   - **Meta Description**: SEO description (optional)
3. Click **Save and Publish**

### Step 6: View Your Blog

Open your browser and navigate to:
- **Homepage**: http://localhost:1549/
- **Blog Post**: Click on any post from the homepage
- **RSS Feed**: http://localhost:1549/feed/rss
- **Sitemap**: http://localhost:1549/sitemap.xml

## Checking Setup Status

To check which Document Types have been created, visit:

**https://localhost:44364/setup/status**

This will show you:
```
📋 Umbraco Setup Status

✅ home: Created
✅ blogPost: Created
✅ author: Created
✅ category: Created
✅ tag: Created

✅ Home page exists: The Silicon Post

To run setup: /setup/run
```

## Troubleshooting

### "Document Type already exists"

If you run `/setup/run` multiple times, it will skip Document Types that already exist. This is safe and won't overwrite your data.

### Can't See Document Types in Backoffice

1. Make sure you completed the Umbraco installation first
2. Try refreshing the backoffice page
3. Log out and log back in
4. Check the application logs in the terminal for any errors

### Can't Create Content

1. Verify Document Types were created: visit `/setup/status`
2. Make sure templates exist in `/Views/*.cshtml`
3. Check that your Document Types have the correct property aliases (they're case-sensitive!)

## What Gets Created?

### Document Types

| Document Type | Alias     | Properties                                                                                      |
| ------------- | --------- | ----------------------------------------------------------------------------------------------- |
| Home          | `home`    | title, metaDescription                                                                          |
| Blog Post     | `blogPost`| title, content, excerpt, featuredImage, publishDate, lastUpdated, author, categories, tags, metaDescription |
| Author        | `author`  | name, bio, avatar, twitterUrl, githubUrl, linkedInUrl                                           |
| Category      | `category`| name, description, slug                                                                         |
| Tag           | `tag`     | name, slug                                                                                      |

### Content Structure

```
📄 The Silicon Post (Home)
├── 📝 Blog Post 1
├── 📝 Blog Post 2
├── 👤 Author 1
├── 👤 Author 2
├── 📁 Category 1
├── 📁 Category 2
├── 🏷️ Tag 1
└── 🏷️ Tag 2
```

### Templates

All templates are already created in the `/Views` folder:
- ✅ Home.cshtml
- ✅ BlogPost.cshtml
- ✅ Author.cshtml
- ✅ Category.cshtml
- ✅ Tag.cshtml

## Manual Setup (Alternative)

If you prefer to create Document Types manually, follow the detailed guide in `UMBRACO_SETUP.md`.

## Next Steps

- **Import WordPress Content**: Follow `MIGRATION_GUIDE.md` to migrate from WordPress
- **Customize Views**: Edit templates in `/Views` folder
- **Add Styling**: Modify `/wwwroot/css/site.css`
- **Deploy**: See `README.md` for deployment instructions

---

**Need Help?** Check the project documentation:
- `README.md` - Project overview
- `UMBRACO_SETUP.md` - Manual setup instructions
- `MIGRATION_GUIDE.md` - WordPress migration guide
- `.github/copilot-instructions.md` - Development patterns and conventions
