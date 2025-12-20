using BookStore.Application.Data;
using BookStore.Application.Interfaces;
using BookStore.Application.Specifications;
using BookStore.Domain.Common;
using BookStore.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Infrastructure.Bases
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _set;
        public GenericRepository(AppDbContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            // Create query with criteria but without paging
            var query = _set.AsQueryable();

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            return await query.CountAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await SoftDeleteAsync(id);
        }

        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _set.Where(predicate).ToListAsync();


        public async Task<IReadOnlyList<T>> GetAllAsync() =>
            await _set.ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }


        //public async Task<IReadOnlyList<T>> GetAllAsyncPaginated(int PageNumber, int PageSize)
        //{

        //    IQueryable<T> query = _set.AsQueryable();

        //    var items = await query
        //        .AsNoTracking()
        //        .Skip((PageNumber - 1) * PageSize)
        //        .Take(PageSize)
        //        .ToListAsync();

        //    return items;
        //}

        //public async Task<IReadOnlyList<T>> GetAllAsyncPaginated(int PageNumber, int PageSize, params Expression<Func<T, object>>[] includes)
        //{
        //    IQueryable<T> query = _set.AsQueryable();

        //    foreach (var include in includes)
        //    {
        //        query = query.Include(include);
        //    }

        //    var items = await query
        //        .AsNoTracking()
        //        .Skip((PageNumber - 1) * PageSize)
        //        .Take(PageSize)
        //        .ToListAsync();

        //    return items;
        //}



        public async Task<T?> GetByIdAsync(Guid id) =>
            await _set.FindAsync(id);

        public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public async Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _set.AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }

        public async Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var totalCount = await query.CountAsync();
            var items = await query
               .AsNoTracking()
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedWithSpecAsync(ISpecification<T> spec)
        {
            // Get total count (without paging)
            var totalCount = await CountAsync(spec);

            // Get paged items
            var items = await ApplySpecification(spec).ToListAsync();

            return (items, totalCount);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_set.AsQueryable(), spec);
        }
        public async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var entity = await _set.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity with id '{id}' was not found.");

            // EF will automatically handle this through SaveChanges override
            _set.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(Guid id)
        {
            var entity = await _set.IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                throw new KeyNotFoundException($"Entity with id '{id}' was not found.");

            // Force hard delete
            _db.Entry(entity).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        public async Task RestoreAsync(Guid id)
        {
            var entity = await _set.IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                throw new KeyNotFoundException($"Entity with id '{id}' was not found.");

            if (!entity.IsDeleted)
                throw new InvalidOperationException($"Entity with id '{id}' is not deleted.");

            entity.IsDeleted = false;
            entity.DeletedAt = null;
            entity.DeletedBy = null;

            await _db.SaveChangesAsync();
        }

        public async Task<T?> GetByIdIncludeDeletedAsync(Guid id)
        {
            return await _set.IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAllIncludeDeletedAsync()
        {
            return await _set.IgnoreQueryFilters().ToListAsync();
        }
    }
}
