using System.Linq.Expressions;

namespace BookStore.Application.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }
        int? Take { get; }
        int? Skip { get; }
        int? Count { get; }
        int? Max { get; }
        int? Min { get; }
        bool IsPagingEnabled { get; }


    }
}
