using AutoMapper;
using BookStore.Application.DTOs.Category;
using BookStore.Application.Features.Categories.Queries.Models;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Categories.Queries.Handlers
{
    public class CategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO?>,
                                        IRequestHandler<GetAllCategoriesQuery, List<CategorySummaryDTO>>
    {
        private readonly IGenericRepository<Category> _repo;
        private readonly IMapper _mapper;
        public CategoryQueryHandler(IGenericRepository<Category> repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<CategoryDTO?> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _repo.GetByIdAsync(request.Id);
            return category is null ? null : _mapper.Map<CategoryDTO?>(category);
        }

        public async Task<List<CategorySummaryDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repo.GetAllAsync();
            if (categories is null || !categories.Any())
                return new List<CategorySummaryDTO>();
            return _mapper.Map<List<CategorySummaryDTO>>(categories);
        }
    }
}
