using AutoMapper;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Books.Commands.Models;
using BookStore.Application.Features.Books.Commands.Validators;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var validationResult = await new CreateBookCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
            var book = _mapper.Map<Book>(request.DTO);
            var created = await _repo.AddAsync(book);
            return _mapper.Map<BookDTO>(created);

        }

        public async Task<BookDTO> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await new UpdateBookCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var book = await _repo.GetByIdAsync(request.Id);

            if (book == null)
                throw new KeyNotFoundException($"Book with id '{request.Id}' was not found.");

            _mapper.Map(request, book);
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
