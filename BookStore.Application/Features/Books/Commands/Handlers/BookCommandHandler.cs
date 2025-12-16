using AutoMapper;
using BookStore.Application.Common;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Books.Commands.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStore.Application.Features.Books.Commands.Handlers
{
    public class BookCommandHandler : IRequestHandler<CreateBookCommand, Result<BookDTO>>,
                                        IRequestHandler<UpdateBookCommand, Result<BookDTO>>,
                                        IRequestHandler<DeleteBookCommand, Result<BookDTO>>
    {
        private readonly IGenericRepository<Book> _repo;
        private readonly IGenericRepository<Author> _authorRepo;
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<BookCommandHandler> _logger;
        public BookCommandHandler(IGenericRepository<Book> repo, IGenericRepository<Author> authorRepo, IGenericRepository<Category> categoryRepo, IMapper mapper, ILogger<BookCommandHandler> logger)
        {
            _repo = repo;
            _authorRepo = authorRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<BookDTO>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("creating book: {title}", request.DTO.Title);
            var author = await _authorRepo.GetByIdAsync(request.DTO.AuthorId);
            if(author == null)
            {
                _logger.LogWarning("author not found: {authorId}", request.DTO.AuthorId);
                return Result<BookDTO>.NotFound("Author", request.DTO.AuthorId);
            }

            var category = await _categoryRepo.GetByIdAsync(request.DTO.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Category not found: {CategoryId}", request.DTO.CategoryId);
                return Result<BookDTO>.NotFound("Category", request.DTO.CategoryId);
            }

            var book = _mapper.Map<Book>(request.DTO);
            var createdBook = await _repo.AddAsync(book);
            _logger.LogInformation("Book created with ID: {BookId}", createdBook.Id);
            var bookDto= _mapper.Map<BookDTO>(createdBook);
            return Result<BookDTO>.Success(bookDto);

        }

        public async Task<Result<BookDTO>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            // var book = await _repo.GetByIdAsync(request.Id);

            // if (book == null)
            //     throw new KeyNotFoundException($"Book with id '{request.Id}' was not found.");

            // _mapper.Map(request.DTO, book);
            // book.Id = request.Id;
            // await _repo.UpdateAsync(book);
            // var dto = _mapper.Map<BookDTO>(book);
            // return dto;
            _logger.LogInformation("Updating book with ID: {BookId}", request.Id);

            var book = await _repo.GetByIdAsync(request.Id);
            if (book == null)
            {
                _logger.LogWarning("Book not found: {BookId}", request.Id);
                return Result<BookDTO>.NotFound("Book", request.Id);
            }

            // Validate author exists
            var author = await _authorRepo.GetByIdAsync(request.DTO.AuthorId);
            if (author == null)
            {
                _logger.LogWarning("Author not found: {AuthorId}", request.DTO.AuthorId);
                return Result<BookDTO>.NotFound("Author", request.DTO.AuthorId);
            }
            // Validate category exists
            var category = await _categoryRepo.GetByIdAsync(request.DTO.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Category not found: {CategoryId}", request.DTO.CategoryId);
                return Result<BookDTO>.NotFound("Category", request.DTO.CategoryId);
            }

            _mapper.Map(request.DTO, book);
            book.Id = request.Id;
            await _repo.UpdateAsync(book);

            _logger.LogInformation("Book updated: {BookId}", request.Id);

            var dto = _mapper.Map<BookDTO>(book);
            return Result<BookDTO>.Success(dto);

        }

        public async Task<Result<BookDTO>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            // if (request == null)
            //     throw new ArgumentNullException(nameof(request));

            // var book = await _repo.GetByIdAsync(request.Id);

            // if (book == null)
            //     return null;

            // var resultDto = _mapper.Map<BookDTO>(book);
            // await _repo.DeleteAsync(book.Id);
            // return resultDto;
             if (request == null)
            {
                _logger.LogError("DeleteBookCommand is null");
                return Result<BookDTO>.Validation("Delete command cannot be null.");
            }

            _logger.LogInformation("Deleting book with ID: {BookId}", request.Id);
           
            var book = await _repo.GetByIdAsync(request.Id);
            if (book == null)
            {
                _logger.LogWarning("Book not found: {BookId}", request.Id);
                return Result<BookDTO>.NotFound("Book", request.Id);
            }

            await _repo.DeleteAsync(book.Id);

            _logger.LogInformation("Book deleted: {BookId}", request.Id);

            var dto = _mapper.Map<BookDTO>(book);
            return Result<BookDTO>.Success(dto);
        }
    }
}
