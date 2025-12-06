using AutoMapper;
using BookStore.Application.DTOs.Author;
using BookStore.Application.DTOs.Book;
using BookStore.Application.Features.Authors.Queries.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Authors.Queries.Handlers
{
    public class AuthorQueryHandler:IRequestHandler<GetAuthorQuery, AuthorDTO?>,
                                    IRequestHandler<GetAllAuthorsQuery, List<AuthorDTO?>>
                                    
    {
        private readonly IGenericRepository<Book> _repo;
        private readonly IMapper _mapper;
        public AuthorQueryHandler(IGenericRepository<Book> repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AuthorDTO?> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            var author = await  _repo.GetByIdAsync(request.Id);
            return author is null ? null : _mapper.Map<AuthorDTO>(author);
        }

        public async Task<List<AuthorDTO?>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = await  _repo.GetAllAsync();
            if (authors is null || !authors.Any())
            {
                return new List<AuthorDTO?>();
            }
            return _mapper.Map<List<AuthorDTO?>>(authors);
        }
    }
}
