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
                                    IRequestHandler<GetAllBooksQuery, List<BookDTO?>>,
                                    IRequestHandler<GetAllBooksPaginatedQuery, List<BookDTO?>>
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

        public async Task<List<BookDTO?>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var books = await _repo.GetAllAsync(b => b.Author, b => b.Category);

            if (books == null)
                return new List<BookDTO?>();

            var dtoList = _mapper.Map<List<BookDTO>>(books);

            return dtoList?.Cast<BookDTO?>().ToList() ?? new List<BookDTO?>();
        }

        public async Task<List<BookDTO?>> Handle(GetAllBooksPaginatedQuery request, CancellationToken cancellationToken)
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
                return PagedResult<BookDTO>.Empty(pageNumber, pageSize);

            var dtoList = _mapper.Map<List<BookDTO>>(items);

            return new PagedResult<BookDTO>(dtoList, totalCount, pageNumber, pageSize);
        }
    }
}
