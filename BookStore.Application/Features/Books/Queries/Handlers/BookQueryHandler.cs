using AutoMapper;
using BookStore.Application.Common;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Books.Queries.Models;
using BookStore.Application.Interfaces;
using BookStore.Application.Specifications;
using BookStore.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStore.Application.Features.Books.Queries.Handlers
{
    public class BookQueryHandler : IRequestHandler<GetBookQuery, BookDTO?>,
                                    IRequestHandler<GetAllBooksQuery, List<BookSummaryDTO?>>,
                                    IRequestHandler<GetAllBooksPaginatedQuery, PagedResult<BookSummaryDTO>>,
                                    IRequestHandler<GetDeletedBooksQuery, List<BookSummaryDTO>>
    {
        private readonly IGenericRepository<Book> _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<BookQueryHandler> _logger;
        public BookQueryHandler(IGenericRepository<Book> repo, IMapper mapper, ILogger<BookQueryHandler> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }
        public async Task<BookDTO?> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var book = await _repo.GetByIdAsync(request.Id, b => b.Author, b => b.Category);
            return book is null ? null : _mapper.Map<BookDTO>(book);
        }

        public async Task<List<BookSummaryDTO?>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var books = await _repo.GetAllAsync(b => b.Author, b => b.Category);

            if (books == null)
                return new List<BookSummaryDTO?>();

            var dtoList = _mapper.Map<List<BookSummaryDTO>>(books);

            return dtoList?.Cast<BookSummaryDTO?>().ToList() ?? new List<BookSummaryDTO?>();
        }

        public async Task<PagedResult<BookSummaryDTO>> Handle(GetAllBooksPaginatedQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Assume request exposes PageNumber and PageSize. Normalize inputs.
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;
            _logger.LogInformation(
                "Getting paginated books. Page: {Page}, Size: {Size}, Search: {Search}, Sort: {Sort}",
                    pageNumber, pageSize, request.SearchTerm, request.SortBy);
            // Create specification
            var filterParams = request.ToFilterParams();
            var spec = new BookSpecification(filterParams);

            // Get paged results using specification
            var (items, totalCount) = await _repo.GetPagedWithSpecAsync(spec);

            _logger.LogInformation(
                "Retrieved {Count} books (Total: {Total}) for page {Page}",
                items.Count, totalCount, pageNumber);

            if (items == null || !items.Any())
                return PagedResult<BookSummaryDTO>.Empty(pageNumber, pageSize);

            var dtoList = _mapper.Map<List<BookSummaryDTO>>(items);

            return new PagedResult<BookSummaryDTO>(dtoList, totalCount, pageNumber, pageSize);
        }

        public async Task<List<BookSummaryDTO>> Handle(GetDeletedBooksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting soft deleted books");

            var deletedBooks = await _repo.GetAllIncludeDeletedAsync();
            var books = deletedBooks.Where(b => b.IsDeleted).ToList();

            var dtoList = _mapper.Map<List<BookSummaryDTO>>(books);
            return dtoList;
        }
    }
}
