using BookStore.Application.Specifications;
using System.Linq.Expressions;

namespace BookStore.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        //Task<IReadOnlyList<T>> GetAllAsyncPaginated(int PageNumber, int PageSize);
        //Task<IReadOnlyList<T>> GetAllAsyncPaginated(int PageNumber, int PageSize, params Expression<Func<T, object>>[] includes);
        Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize);

        Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            params Expression<Func<T, object>>[] includes);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedWithSpecAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Soft delete
        Task SoftDeleteAsync(Guid id);

        // Hard delete (use with caution!)
        Task HardDeleteAsync(Guid id);

        // Restore soft deleted item
        Task RestoreAsync(Guid id);

        // Get including soft deleted items
        Task<T?> GetByIdIncludeDeletedAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllIncludeDeletedAsync();
    }
}
