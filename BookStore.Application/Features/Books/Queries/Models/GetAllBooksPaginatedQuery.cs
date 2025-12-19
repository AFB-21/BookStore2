using BookStore.Application.Common;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Specifications;
using MediatR;

namespace BookStore.Application.Features.Books.Queries.Models
{
    public record GetAllBooksPaginatedQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        Guid? AuthorId = null,
        Guid? CategoryId = null,
        string? SortBy = null,
        bool IsDescending = false
        ) : IRequest<PagedResult<BookDTO?>>
    {
        public BookFilterParams ToFilterParams() => new()
        {
            PageNumber = PageNumber,
            PageSize = PageSize,
            SearchTerm = SearchTerm,
            MinPrice = MinPrice,
            MaxPrice = MaxPrice,
            AuthorId = AuthorId,
            CategoryId = CategoryId,
            SortBy = SortBy,
            IsDescending = IsDescending
        };
    }


}
