using AutoMapper;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Books.Queries.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Books.Queries.Handlers
{
    public class BookQueryHandler : IRequestHandler<GetBookQuery, BookDTO?>,
                                    IRequestHandler<GetAllBooksQuery, List<BookDTO?>>,
                                    IRequestHandler<GetAllBooksPaginatedQuery, List<BookDTO?>>
    {
        private readonly IGenericRepository<Book> _repo;
        private readonly IMapper _mapper;
        public BookQueryHandler(IGenericRepository<Book> repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

            var books = await _repo.GetAllAsyncPaginated(pageNumber, pageSize, b => b.Author, b => b.Category);

            if (books == null || !books.Any())
                return new List<BookDTO?>();

            //var skip = (pageNumber - 1) * pageSize;
            //var paged = books.Skip(skip).Take(pageSize).ToList();

            var dtoList = _mapper.Map<List<BookDTO>>(books);

            return dtoList?.Cast<BookDTO?>().ToList() ?? new List<BookDTO?>();
        }
    }
}
