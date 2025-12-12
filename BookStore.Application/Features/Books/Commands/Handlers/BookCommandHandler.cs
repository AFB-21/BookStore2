using AutoMapper;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Books.Commands.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Books.Commands.Handlers
{
    public class BookCommandHandler : IRequestHandler<CreateBookCommand, BookDTO>,
                                        IRequestHandler<UpdateBookCommand, BookDTO>,
                                        IRequestHandler<DeleteBookCommand, BookDTO>
    {
        private readonly IGenericRepository<Book> _repo;
        private readonly IMapper _mapper;
        public BookCommandHandler(IGenericRepository<Book> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<BookDTO> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request.DTO);
            var createdBook = await _repo.AddAsync(book);
            return _mapper.Map<BookDTO>(createdBook);

        }

        public async Task<BookDTO> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {

            var book = await _repo.GetByIdAsync(request.Id);

            if (book == null)
                throw new KeyNotFoundException($"Book with id '{request.Id}' was not found.");

            _mapper.Map(request.DTO, book);
            book.Id = request.Id;
            await _repo.UpdateAsync(book);
            var dto = _mapper.Map<BookDTO>(book);
            return dto;
        }

        public async Task<BookDTO> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var book = await _repo.GetByIdAsync(request.Id);

            if (book == null)
                return null;

            var resultDto = _mapper.Map<BookDTO>(book);
            await _repo.DeleteAsync(book.Id);
            return resultDto;
        }
    }
}
