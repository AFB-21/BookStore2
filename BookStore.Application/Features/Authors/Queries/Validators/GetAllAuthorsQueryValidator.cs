using BookStore.Application.Features.Authors.Queries.Models;
using FluentValidation;

namespace BookStore.Application.Features.Authors.Queries.Validators
{
    public class GetAllAuthorsQueryValidator : AbstractValidator<GetAllAuthorsQuery>
    {
    }
}
