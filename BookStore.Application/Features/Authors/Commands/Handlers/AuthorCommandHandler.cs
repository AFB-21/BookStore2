using AutoMapper;
using BookStore.Application.Common;
using BookStore.Application.DTOs.Author;
using BookStore.Application.Features.Authors.Commands.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStore.Application.Features.Authors.Commands.Handlers
{
    public class AuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Result<AuthorDTO>>,
                                      IRequestHandler<UpdateAuthorCommand, Result<AuthorDTO>>,
                                      IRequestHandler<DeleteAuthorCommand, Result<AuthorDTO>>
    {
        private readonly IGenericRepository<Author> _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorCommandHandler> _logger;
        public AuthorCommandHandler(IGenericRepository<Author> repo, IMapper mapper, ILogger<AuthorCommandHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<AuthorDTO>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating author: {Name}", request.DTO.Name);

            var author = _mapper.Map<Author>(request.DTO);
            var createdAuthor = await _repo.AddAsync(author);
            _logger.LogInformation("Author created with ID: {AuthorId}", createdAuthor.Id);
            var authorDto = _mapper.Map<AuthorDTO>(createdAuthor);
            return Result<AuthorDTO>.Success(authorDto);
        }

        public async Task<Result<AuthorDTO>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating author: {AuthorId}", request.Id);

            var author = await _repo.GetByIdAsync(request.Id);
            if (author == null)
            {
                _logger.LogWarning("Author not found: {AuthorId}", request.Id);
                return Result<AuthorDTO>.NotFound("Author", request.Id);
            }

            _mapper.Map(request.DTO, author);
            author.Id = request.Id;
            await _repo.UpdateAsync(author);

            _logger.LogInformation("Author updated: {AuthorId}", request.Id);
            var updatedAuthor = _mapper.Map<AuthorDTO>(author);
            return Result<AuthorDTO>.Success(updatedAuthor);
        }

        public async Task<Result<AuthorDTO>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogError("DeleteAuthorCommand is null");
                return Result<AuthorDTO>.Validation("Delete command cannot be null.");
            }

            _logger.LogInformation("Deleting author: {AuthorId}", request.Id);

            var author = await _repo.GetByIdAsync(request.Id);
            if (author == null)
            {
                _logger.LogWarning("Author not found: {AuthorId}", request.Id);
                return Result<AuthorDTO>.NotFound("Author", request.Id);
            }
            // Check if author has books
            if (author.Books != null && author.Books.Any())
            {
                _logger.LogWarning("Cannot delete author with books: {AuthorId}", request.Id);
                return Result<AuthorDTO>.Conflict(
                    $"Cannot delete author '{author.Name}' because they have {author.Books.Count} book(s).");
            }

            var deletedAuthor = _mapper.Map<AuthorDTO>(author);
            await _repo.DeleteAsync(author.Id);

            _logger.LogInformation("Author deleted: {AuthorId}", request.Id);

            return Result<AuthorDTO>.Success(deletedAuthor);
        }
    }
}
