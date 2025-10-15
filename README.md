# The Silicon Post

A modern tech blog built with **Umbraco CMS v16.2** on **.NET 9**, migrating from WordPress to a high-performance C# solution.

## 🎯 Project Overview

This project rebuilds The Silicon Post tech blog using Umbraco CMS, replacing WordPress with a modern .NET-based content management system. The goal is to achieve better performance, security, and maintainability while preserving all essential blogging features.

### Why Migrate from WordPress to Umbraco?

- ⚡ **Performance**: Native .NET performance vs. PHP overhead
- 🔒 **Security**: Reduced attack surface, no plugin vulnerabilities
- 🛠️ **Developer Control**: Full control over code and architecture
- 📦 **Modern Stack**: Built on .NET 9 with C# 12
- 🎨 **Flexible Content**: Powerful Block Grid editor for rich content

## ✨ Features

### ✅ Implemented

- **RSS Feed** (`/feed/rss`) - Full RSS 2.0 feed generation
- **XML Sitemap** (`/sitemap.xml`) - SEO-optimized sitemap
- **SEO Optimization**
  - OpenGraph tags for social sharing
  - Twitter Card metadata
  - Schema.org JSON-LD structured data
  - Meta descriptions and canonical URLs
- **Content Management**
  - Blog posts with categories and tags
  - Author profiles with social links
  - Featured images
  - Rich content editing with Block Grid
- **Umbraco Backoffice** - Modern content editing interface

### 🚧 Roadmap

- [ ] WordPress content migration tool
- [ ] URL redirect mapping from old WordPress structure
- [ ] Search functionality
- [ ] Comments system (or third-party integration)
- [ ] Related posts algorithm
- [ ] Custom post templates/layouts
- [ ] Newsletter integration
- [ ] Performance optimization (caching, CDN)

## 🏗️ Architecture

### Technology Stack

- **CMS**: Umbraco v16.2
- **Framework**: .NET 9
- **Language**: C# 12
- **Database**: SQL Server (via Umbraco)
- **Frontend**: Razor Views + Block Grid

### Project Structure

```
TheSiliconPost/
├── Controllers/          # Custom routes (RSS, Sitemap)
├── Helpers/             # SEO and utility helpers
├── Models/              # Strongly-typed content models
├── Views/               # Razor views and partials
├── wwwroot/             # Static files
├── .github/             # GitHub workflows and AI instructions
└── appsettings.json     # Configuration
```

### Key Patterns

**Strongly-Typed Content Models**: All content types inherit from `PublishedContentWrapped`

```csharp
public class BlogPost : PublishedContentWrapped
{
    public string Title => this.Value<string>("title") ?? this.Name;
    public DateTime PublishDate => this.Value<DateTime>("publishDate");
}
```

**Custom Controllers**: Use `SurfaceController` for custom functionality

```csharp
[Route("feed/rss")]
public class RssFeedController : SurfaceController
{
    // Custom RSS feed generation
}
```

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server or SQL Server LocalDB
- IDE: Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/solgi3/TheSiliconPost.git
   cd TheSiliconPost
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Run the application**

   ```bash
   dotnet run
   ```

4. **Access Umbraco**
   - Navigate to `https://localhost:5001/umbraco`
   - Complete the Umbraco installation wizard
   - Create your admin account

### Configuration

Update `appsettings.json` with your site settings:

```json
{
  "SiteSettings": {
    "Url": "https://yourdomain.com",
    "Name": "Your Site Name",
    "Description": "Your site description"
  }
}
```

## 📝 Content Types

### BlogPost

- Title, Content, Excerpt
- Publish Date, Last Updated
- Featured Image, OG Image
- Categories, Tags
- SEO fields (meta description, keywords)

### Author

- Name, Bio, Email
- Social links (Twitter, GitHub, LinkedIn)
- Avatar image

### Category & Tag

- Name, Description, Slug

## 🔧 Development

### Running in Development Mode

```bash
dotnet watch run
```

### Key URLs

- **Homepage**: `https://localhost:5001`
- **Umbraco Backoffice**: `https://localhost:5001/umbraco`
- **RSS Feed**: `https://localhost:5001/feed/rss`
- **Sitemap**: `https://localhost:5001/sitemap.xml`

### Adding New Features

See [`.github/copilot-instructions.md`](.github/copilot-instructions.md) for detailed development guidelines and patterns.

## 📦 Dependencies

- **Umbraco.Cms** (v16.2.0) - Core CMS functionality
- **Microsoft.ICU.ICU4C.Runtime** - Globalization support

## 🤝 Contributing

This is a personal blog migration project, but suggestions and feedback are welcome!

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is private and not licensed for public use.

## 🙏 Acknowledgments

- **Umbraco CMS** for the excellent .NET CMS platform
- The .NET community for continued innovation
- WordPress for 15+ years of service (but it's time to move on 😊)

## 📞 Contact

**Mohammad Yosef Solgi**

- GitHub: [@solgi3](https://github.com/solgi3)
- Email: y.solgi3@gmail.com

---

Built with ❤️ using Umbraco CMS and .NET 9
