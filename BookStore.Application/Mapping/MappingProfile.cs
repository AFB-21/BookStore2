using AutoMapper;
using BookStore.Application.DTOs.Author;
using BookStore.Application.DTOs.Book;
using BookStore.Application.DTOs.Category;
using BookStore.Domain.Entities;

namespace BookStore.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Book Mappings
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                //.ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Name))
                //.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();

            CreateMap<CreateBookDTO, Book>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId));
            CreateMap<UpdateBookDTO, Book>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId));

            //Author Mappings
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<CreateAuthorDTO, Author>();
            CreateMap<UpdateAuthorDTO, Author>();

            //Category Mappings
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();
        }
    }
}
