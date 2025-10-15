using TheSiliconPost.ViewModels;

namespace TheSiliconPost.Helpers
{
    /// <summary>
    /// Helper class for paginating collections
    /// </summary>
    public static class PaginationHelper
    {
        /// <summary>
        /// Paginate a collection of items
        /// </summary>
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> items, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            
            return items
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
        
        /// <summary>
        /// Create a BlogListingViewModel with pagination
        /// </summary>
        public static BlogListingViewModel CreateBlogListing<T>(
            IEnumerable<T> allItems,
            int currentPage,
            int pageSize = 10,
            string? searchQuery = null,
            string? selectedCategory = null,
            string? selectedTag = null,
            string title = "Blog",
            string? description = null,
            string baseUrl = "/blog")
        {
            var itemsList = allItems.ToList();
            var totalItems = itemsList.Count;
            var paginatedItems = itemsList.Paginate(currentPage, pageSize);
            
            return new BlogListingViewModel
            {
                Posts = paginatedItems.Cast<dynamic>(), // Will work with BlogPost items
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                SearchQuery = searchQuery,
                SelectedCategory = selectedCategory,
                SelectedTag = selectedTag,
                Title = title,
                Description = description,
                BaseUrl = baseUrl
            };
        }
        
        /// <summary>
        /// Get pagination metadata for API responses
        /// </summary>
        public static PaginationMetadata GetMetadata(int totalItems, int currentPage, int pageSize)
        {
            return new PaginationMetadata
            {
                TotalItems = totalItems,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                HasPreviousPage = currentPage > 1,
                HasNextPage = currentPage < (int)Math.Ceiling((double)totalItems / pageSize)
            };
        }
    }
    
    /// <summary>
    /// Pagination metadata
    /// </summary>
    public class PaginationMetadata
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
