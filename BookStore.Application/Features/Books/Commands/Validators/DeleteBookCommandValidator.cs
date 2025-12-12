using BookStore.Application.Features.Books.Commands.Models;
using FluentValidation;

namespace BookStore.Application.Features.Books.Commands.Validators
{
    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Book Id is required.");
        }
    }
}
