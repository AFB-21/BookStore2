using AutoMapper;
using BookStore.Application.DTOs.Author;
using BookStore.Application.Features.Authors.Queries.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Authors.Queries.Handlers
{
    public class AuthorQueryHandler : IRequestHandler<GetAuthorQuery, AuthorDTO?>,
                                    IRequestHandler<GetAllAuthorsQuery, List<AuthorSummaryDTO>>
    {
        private readonly IGenericRepository<Author> _repo;
        private readonly IMapper _mapper;
        public AuthorQueryHandler(IGenericRepository<Author> repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AuthorDTO?> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            var author = await _repo.GetByIdAsync(request.Id, a => a.Books);
            return author is null ? null : _mapper.Map<AuthorDTO>(author);
        }

        public async Task<List<AuthorSummaryDTO>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = await _repo.GetAllAsync(a => a.Books);
            if (authors is null || !authors.Any())
            {
                return new List<AuthorSummaryDTO>();
            }
            return _mapper.Map<List<AuthorSummaryDTO>>(authors);
        }
    }
}
