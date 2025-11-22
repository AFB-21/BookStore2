using BookStore.Application.Data;
using BookStore.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Bases
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _set;
        public GenericRepository(AppDbContext db)
        {
            _db = db;
            _set =db.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _set.FindAsync(id);
            _set.Remove(entity);
            await _db.SaveChangesAsync();
        }
           
        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _set.Where(predicate).ToListAsync();
                

        public async Task<IReadOnlyList<T>> GetAllAsync() =>
            await _set.ToListAsync();
        public async Task<IReadOnlyList<T>> GetAllAsyncPaginated()
        {
           return await _set.ToListAsync();
        }
            
       

        public async Task<T?> GetByIdAsync(Guid id) =>
            await _set.FindAsync(id);

        public async Task UpdateAsync(T entity)
        { 
             _set.Update(entity);
            await _db.SaveChangesAsync();
}
    }
}
