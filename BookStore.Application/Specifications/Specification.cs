using System.Linq.Expressions;

namespace BookStore.Application.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>>? Criteria { get; private set; }

        public List<Expression<Func<T, object>>> Includes { get; } = new();

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        public int? Take { get; private set; }

        public int? Skip { get; private set; }

        public int? Count { get; private set; }

        public int? Max { get; private set; }

        public int? Min { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void AddCriteria(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
        protected void ApplyCount(int count)
        {
            Count = count;
        }
    }
}
