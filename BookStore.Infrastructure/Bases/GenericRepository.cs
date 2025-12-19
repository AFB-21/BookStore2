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

        public async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
