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
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDTO>
    {
        private readonly IGenericRepository<Book> _repo;
        private readonly IMapper _mapper;
        public CreateBookCommandHandler(IGenericRepository<Book> repo, IMapper mapper)
        {
         _repo = repo;
         _mapper = mapper;
        }
        public async Task<BookDTO> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book= _mapper.Map<Book>(request.DTO);
            var created= await _repo.AddAsync(book);
            return _mapper.Map<BookDTO>(created);
        }
    }
}
