using AutoMapper;
using BookStore.Application.DTOs.Category;
using BookStore.Application.Features.Categories.Commands.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Categories.Commands.Handlers
{
    public class CategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDTO>,
                                        IRequestHandler<UpdateCategoryCommand, CategoryDTO>,
                                        IRequestHandler<DeleteCategoryCommand, CategoryDTO>
    {
        private readonly IGenericRepository<Category> _repo;
        private readonly IMapper _mapper;
        public CategoryCommandHandler(IGenericRepository<Category> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<CategoryDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request.DTO);
            var createdCategory = await _repo.AddAsync(category);
            return _mapper.Map<CategoryDTO>(createdCategory);

        }

        public async Task<CategoryDTO> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repo.GetByIdAsync(request.Id);
            if (category == null)
                return null;
            _mapper.Map(request.DTO, category);
            category.Id = request.Id;
            await _repo.UpdateAsync(category);
            var updatedCategory = _mapper.Map<CategoryDTO>(category);
            return updatedCategory;
        }

        public async Task<CategoryDTO> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repo.GetByIdAsync(request.Id);
            if (category == null)
                return null;
            var deletedCategory = _mapper.Map<CategoryDTO>(category);
            await _repo.DeleteAsync(category.Id);
            return deletedCategory;
        }
    }
}
