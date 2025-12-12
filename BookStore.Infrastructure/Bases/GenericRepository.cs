using BookStore.Application.Data;
using BookStore.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Infrastructure.Bases
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
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

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _set.FindAsync(new object[] { id });
            if (entity == null)
                throw new KeyNotFoundException($"Entity with id '{id}' was not found.");

            _set.Remove(entity);
            await _db.SaveChangesAsync();
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

        // Plan / Pseudocode:
        // 1. Define default pagination parameters (page number, page size). These can be adjusted later
        //    or replaced by reading configuration or passing parameters to the method.
        // 2. Build an IQueryable<T> from the DbSet<T>.
        // 3. Apply AsNoTracking() for read-only performance.
        // 4. Apply Skip((pageNumber-1) * pageSize) and Take(pageSize) to get one page.
        // 5. Execute the query with ToListAsync and return the result as IReadOnlyList<T>.
        // Note: method signature has no parameters, so defaults are applied here to provide pagination.
        public async Task<IReadOnlyList<T>> GetAllAsyncPaginated(int PageNumber, int PageSize)
        {

            IQueryable<T> query = _set.AsQueryable();

            var items = await query
                .AsNoTracking()
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return items;
        }

        public async Task<IReadOnlyList<T>> GetAllAsyncPaginated(int PageNumber, int PageSize, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set.AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var items = await query
                .AsNoTracking()
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return items;
        }



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

        public async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
