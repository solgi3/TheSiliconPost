using TheSiliconPost.Models;

namespace TheSiliconPost.ViewModels
{
    /// <summary>
    /// ViewModel for blog listing pages with pagination support
    /// </summary>
    public class BlogListingViewModel
    {
        public IEnumerable<BlogPost> Posts { get; set; } = Enumerable.Empty<BlogPost>();
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public string? SearchQuery { get; set; }
        public string? SelectedCategory { get; set; }
        public string? SelectedTag { get; set; }
        
        // Helper properties
        public string Title { get; set; } = "Blog";
        public string? Description { get; set; }
        public string BaseUrl { get; set; } = "/blog";
        
        /// <summary>
        /// Get page numbers for pagination display
        /// </summary>
        public IEnumerable<int> GetPageNumbers(int maxPages = 5)
        {
            var pages = new List<int>();
            var startPage = Math.Max(1, CurrentPage - maxPages / 2);
            var endPage = Math.Min(TotalPages, startPage + maxPages - 1);
            
            // Adjust start if we're near the end
            if (endPage - startPage < maxPages - 1)
            {
                startPage = Math.Max(1, endPage - maxPages + 1);
            }
            
            for (int i = startPage; i <= endPage; i++)
            {
                pages.Add(i);
            }
            
            return pages;
        }
        
        /// <summary>
        /// Build query string for pagination links
        /// </summary>
        public string GetQueryString(int page)
        {
            var queryParams = new List<string>();
            
            if (page > 1)
                queryParams.Add($"page={page}");
            
            if (!string.IsNullOrEmpty(SearchQuery))
                queryParams.Add($"q={Uri.EscapeDataString(SearchQuery)}");
            
            if (!string.IsNullOrEmpty(SelectedCategory))
                queryParams.Add($"category={Uri.EscapeDataString(SelectedCategory)}");
            
            if (!string.IsNullOrEmpty(SelectedTag))
                queryParams.Add($"tag={Uri.EscapeDataString(SelectedTag)}");
            
            return queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
        }
    }
}
