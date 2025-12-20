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
            // ========== BOOK MAPPINGS ==========

            // Full Book DTO (for single book view)
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Author != null ? src.Author.Name : "Unknown"))
                .ForMember(dest => dest.AuthorBio,
                    opt => opt.MapFrom(src => src.Author != null ? src.Author.Bio : null))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Unknown"));

            // Book Summary (for lists/collections)
            CreateMap<Book, BookSummaryDTO>()
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Author != null ? src.Author.Name : "Unknown"))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Unknown"));

            // Book Card (for UI tiles)
            CreateMap<Book, BookCardDTO>()
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Author != null ? src.Author.Name : "Unknown"))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Unknown"));

            // Create/Update mappings
            CreateMap<CreateBookDTO, Book>();
            CreateMap<UpdateBookDTO, Book>();
            CreateMap<BookDTO, Book>().ReverseMap();

            // ========== AUTHOR MAPPINGS ==========

            // Full Author DTO (for single author view)
            CreateMap<Author, AuthorDTO>()
                .ForMember(dest => dest.Books,
                    opt => opt.MapFrom(src => src.Books));  // Maps to BookSummaryDTO

            // Author Summary (for lists)
            CreateMap<Author, AuthorSummaryDTO>()
                .ForMember(dest => dest.BookCount,
                    opt => opt.MapFrom(src => src.Books != null ? src.Books.Count : 0));

            // Create/Update mappings
            CreateMap<CreateAuthorDTO, Author>();
            CreateMap<UpdateAuthorDTO, Author>();

            // ========== CATEGORY MAPPINGS ==========

            // Full Category DTO (for single category view)
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.Books,
                    opt => opt.MapFrom(src => src.Books));  // Maps to BookSummaryDTO

            // Category Summary (for lists)
            CreateMap<Category, CategorySummaryDTO>()
                .ForMember(dest => dest.BookCount,
                    opt => opt.MapFrom(src => src.Books != null ? src.Books.Count : 0));

            // Create/Update mappings
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();
        }
    }
}
