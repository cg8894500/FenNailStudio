using System;
using Microsoft.EntityFrameworkCore;
using FenNailStudio.Domain.Interfaces;
using FenNailStudio.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace FenNailStudio.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly FenNailStudioDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(FenNailStudioDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
