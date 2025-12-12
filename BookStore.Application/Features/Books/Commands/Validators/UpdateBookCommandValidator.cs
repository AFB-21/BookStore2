using BookStore.Application.Features.Books.Commands.Models;
using FluentValidation;

namespace BookStore.Application.Features.Books.Commands.Validators
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {

            RuleFor(x => x.DTO.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DTO.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.DTO.AuthorId).NotEmpty();
            RuleFor(x => x.DTO.CategoryId).NotEmpty();
        }
    }
}
