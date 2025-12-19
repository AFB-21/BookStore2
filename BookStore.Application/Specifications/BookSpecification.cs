using BookStore.Domain.Entities;

namespace BookStore.Application.Specifications
{
    public class BookSpecification : Specification<Book>
    {
        public BookSpecification(BookFilterParams filterParams)
        {
            AddInclude(b => b.Author);
            AddInclude(b => b.Category);

            if (!string.IsNullOrEmpty(filterParams.SearchTerm))
            {
                var searchTerm = filterParams.SearchTerm.ToLower();
                AddCriteria(b =>
                    b.Title.ToLower().Contains(searchTerm) ||
                    (b.Description != null && b.Description.ToLower().Contains(searchTerm)));
            }

            if (filterParams.MinPrice.HasValue)
            {
                AddCriteria(b => b.Price >= filterParams.MinPrice.Value);
            }

            if (filterParams.MaxPrice.HasValue)
            {
                AddCriteria(b => b.Price <= filterParams.MaxPrice.Value);
            }

            if (filterParams.AuthorId.HasValue)
            {
                AddCriteria(b => b.AuthorId == filterParams.AuthorId.Value);
            }

            if (filterParams.CategoryId.HasValue)
            {
                AddCriteria(b => b.CategoryId == filterParams.CategoryId.Value);
            }

            // Apply sorting
            switch (filterParams.SortBy?.ToLower())
            {
                case "price":
                    if (filterParams.IsDescending)
                        AddOrderByDescending(b => b.Price);
                    else
                        AddOrderBy(b => b.Price);
                    break;

                case "title":
                    if (filterParams.IsDescending)
                        AddOrderByDescending(b => b.Title);
                    else
                        AddOrderBy(b => b.Title);
                    break;

                case "date":
                case "publishedon":
                    if (filterParams.IsDescending)
                        AddOrderByDescending(b => b.PublishedOn);
                    else
                        AddOrderBy(b => b.PublishedOn);
                    break;

                default:
                    AddOrderBy(b => b.Title); // Default sort
                    break;
            }

            // Apply pagination
            if (filterParams.PageNumber > 0 && filterParams.PageSize > 0)
            {
                ApplyPaging(
                    (filterParams.PageNumber - 1) * filterParams.PageSize,
                    filterParams.PageSize);
            }
        }


    }
    public class BookFilterParams
    {
        public string? SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
