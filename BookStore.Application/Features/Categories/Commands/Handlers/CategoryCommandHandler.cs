using AutoMapper;
using BookStore.Application.Common;
using BookStore.Application.DTOs.Category;
using BookStore.Application.Features.Categories.Commands.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStore.Application.Features.Categories.Commands.Handlers
{
    public class CategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDTO>>,
                                        IRequestHandler<UpdateCategoryCommand, Result<CategoryDTO>>,
                                        IRequestHandler<DeleteCategoryCommand, Result<CategoryDTO>>
    {
        private readonly IGenericRepository<Category> _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryCommandHandler> _logger;
        public CategoryCommandHandler(IGenericRepository<Category> repo, IMapper mapper, ILogger<CategoryCommandHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<CategoryDTO>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating category with name: {CategoryName}", request.DTO.Name);
            if (request == null)
            {
                _logger.LogError("CreateCategoryCommand is null");
                return Result<CategoryDTO>.Forbidden();
            }
            var category = _mapper.Map<Category>(request.DTO);
            var createdCategory = await _repo.AddAsync(category);
            return Result<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(createdCategory));

        }

        public async Task<Result<CategoryDTO>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repo.GetByIdAsync(request.Id);
            if (category == null)
                return Result<CategoryDTO>.NotFound("not found",request.Id);
            _mapper.Map(request.DTO, category);
            category.Id = request.Id;
            await _repo.UpdateAsync(category);
            var updatedCategory = _mapper.Map<CategoryDTO>(category);
            return updatedCategory;
        }

        public async Task<Result<CategoryDTO>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repo.GetByIdAsync(request.Id);
            if (category == null)
                return Result<CategoryDTO>.NotFound("not found", request.Id);
            var deletedCategory = _mapper.Map<CategoryDTO>(category);
            await _repo.DeleteAsync(category.Id);
            return deletedCategory;
        }
    }
}
