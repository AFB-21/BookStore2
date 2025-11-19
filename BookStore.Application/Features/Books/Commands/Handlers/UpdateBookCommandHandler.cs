using AutoMapper;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Books.Commands.Models;
using BookStore.Application.Features.Books.Commands.Validators;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Books.Commands.Handlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDTO>
    {
        private readonly IGenericRepository<Book> _repo;
        private readonly IMapper _mapper;
        public UpdateBookCommandHandler(IGenericRepository<Book> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /*
         PSEUDOCODE / PLAN (detailed):
         1. Validate the incoming UpdateBookCommand using UpdateBookCommandValidator.
            - If validation fails, throw a FluentValidation.ValidationException with the errors.
         2. Retrieve the existing Book entity from the repository by request.Id.
            - If no entity is found, throw a KeyNotFoundException (or a suitable not-found exception).
         3. Map the incoming request data onto the existing Book entity to update its properties.
            - Use AutoMapper's Map(source, destination) to apply only the changed fields.
            - Ensure the entity's Id remains unchanged (set it from request.Id if needed).
         4. Call the repository's UpdateAsync to persist the changes.
         5. Map the updated entity to BookDTO and return it.

         Notes:
         - The handler assumes an AutoMapper mapping exists between UpdateBookCommand (or its inner DTO)
           and Book, and between Book and BookDTO.
         - Repository methods do not accept a CancellationToken per provided signatures.
        */

        public async Task<BookDTO> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate
            var validationResult = await new UpdateBookCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // 2. Retrieve existing entity
            var book = await _repo.GetByIdAsync(request.Id);
            if (book == null)
                throw new KeyNotFoundException($"Book with id '{request.Id}' was not found.");

            // 3. Map incoming values onto existing entity
            // This uses AutoMapper to update the existing entity with values from the request.
            // It requires a mapping configuration from UpdateBookCommand (or its DTO) -> Book.
            _mapper.Map(request, book);

            // Ensure the Id stays correct in case mapping didn't preserve it
            book.Id = request.Id;

            // 4. Persist changes
            await _repo.UpdateAsync(book);

            // 5. Map to DTO and return
            var dto = _mapper.Map<BookDTO>(book);
            return dto;
        }
    }
}
