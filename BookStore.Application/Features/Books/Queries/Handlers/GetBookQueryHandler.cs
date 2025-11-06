using AutoMapper;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Books.Queries.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Books.Queries.Handlers
{
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDTO?>
    {
        private readonly IGenericRepository<Book> _repo;
        private readonly IMapper _mapper;
        public GetBookQueryHandler(IGenericRepository<Book> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<BookDTO?> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var book = await _repo.GetByIdAsync(request.Id);
            return book is null ? null : _mapper.Map<BookDTO>(book);
        }
    }
}
