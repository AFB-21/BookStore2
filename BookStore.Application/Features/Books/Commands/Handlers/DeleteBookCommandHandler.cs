using AutoMapper;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Books.Commands.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Books.Commands.Handlers
{
    // Plan (pseudocode):
    // 1. Inject IBookRepository and IMapper via constructor.
    // 2. In Handle:
    //    a. Validate request (ensure id provided).
    //    b. Load existing book from repository by id.
    //    c. If not found, return null (or throw if preferred).
    //    d. Map entity to BookDTO to return after deletion.
    //    e. Delete the entity using repository and persist changes.
    //    f. Return the mapped BookDTO.
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, BookDTO>
    {
        private readonly IGenericRepository<Book> _repo;
        private readonly IMapper _mapper;
        public DeleteBookCommandHandler(IGenericRepository<Book> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<BookDTO> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Attempt to fetch the existing book
            var book = await _repo.GetByIdAsync(request.Id);

            // If not found, return null (caller can decide how to handle)
            if (book == null)
                return null;

            // Map entity to DTO before deletion so we can return the detached data
            var resultDto = _mapper.Map<BookDTO>(book);

            // Delete and persist
            await _repo.DeleteAsync(book.Id);

            return resultDto;
        }
    }
}
