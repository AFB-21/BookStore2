using AutoMapper;
using BookStore.Application.DTOs.Book;
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
            CreateMap<CreateBookDTO, Book>();
            //CreateMap<Author, AuthorDTO>().ReverseMap();
            //CreateMap<Category, CategoryDTO>().ReverseMap();

        }
    }
}
