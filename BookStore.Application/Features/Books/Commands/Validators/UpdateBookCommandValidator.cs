using BookStore.Application.Features.Books.Commands.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Features.Books.Commands.Validators
{
    public class UpdateBookCommandValidator: AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Book Id is required.");
            RuleFor(x => x.DTO.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DTO.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.DTO.AuthorId).NotEmpty();
            RuleFor(x => x.DTO.CategoryId).NotEmpty();
        }
    }
}
