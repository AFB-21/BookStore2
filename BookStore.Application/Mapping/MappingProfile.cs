using AutoMapper;
using BookStore.Application.DTOs.Author;
using BookStore.Application.DTOs.Book;
using BookStore.Application.DTOs.Category;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Book,BookDTO>().ReverseMap();
            CreateMap<CreateBookDTO, Book>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src=>src.CategoryId))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src=>src.AuthorId));
            CreateMap<UpdateBookDTO, Book>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId));

            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<CreateAuthorDTO, Author>();
            CreateMap<UpdateAuthorDTO, Author>();

            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();

        }
    }
}
