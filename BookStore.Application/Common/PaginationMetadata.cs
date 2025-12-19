namespace BookStore.Application.Common
{
    public class PaginationMetadata
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public PaginationMetadata(int totalCount, int currentPage, int pageSize)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            HasPrevious = currentPage > 1;
            HasNext = currentPage < TotalPages;
        }
    }
}
