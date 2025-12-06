using AutoMapper;
using BookStore.Application.DTOs.Author;
using BookStore.Application.Features.Authors.Commands.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Authors.Commands.Handlers
{
    public class AuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDTO>,
                                      IRequestHandler<UpdateAuthorCommand, AuthorDTO>,
                                      IRequestHandler<DeleteAuthorCommand, AuthorDTO>
    {
        private readonly IGenericRepository<Author> _repo;
        private readonly IMapper _mapper;
        public AuthorCommandHandler(IGenericRepository<Author> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<AuthorDTO> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = _mapper.Map<Author>(request.DTO);
            var createdAuthor= await _repo.AddAsync(author);
            return _mapper.Map<AuthorDTO>(createdAuthor);
        }

        public async Task<AuthorDTO> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author= await _repo.GetByIdAsync(request.Id);
            if (author == null)
                return null;
            _mapper.Map(request.DTO, author);
            author.Id = request.Id;
             await _repo.UpdateAsync(author);
            var updatedAuthor = _mapper.Map<AuthorDTO>(author);
            return updatedAuthor;
        }

        public async Task<AuthorDTO> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var author = await _repo.GetByIdAsync(request.Id);

            if (author == null)
                return null;
            var deletedAuthor = _mapper.Map<AuthorDTO>(author);
            await _repo.DeleteAsync(author.Id);
            return deletedAuthor;
        }
    }
}
