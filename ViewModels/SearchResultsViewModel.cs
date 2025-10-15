using TheSiliconPost.Models;

namespace TheSiliconPost.ViewModels
{
    /// <summary>
    /// ViewModel for search results
    /// </summary>
    public class SearchResultsViewModel
    {
        public string Query { get; set; } = string.Empty;
        public IEnumerable<BlogPost> Results { get; set; } = Enumerable.Empty<BlogPost>();
        public int TotalResults { get; set; }
        public int PageSize { get; set; } = 20;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        
        /// <summary>
        /// Search execution time in milliseconds
        /// </summary>
        public long ExecutionTimeMs { get; set; }
    }
}
