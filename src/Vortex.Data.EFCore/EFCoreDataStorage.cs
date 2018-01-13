using Equilaterus.Vortex.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services.EFCore
{
    public class EFCoreDataStorage<T> : IDataStorage<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public EFCoreDataStorage(DbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }              

        public async Task<List<T>> FindAllAsync(
            params string[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            query = query.AddIncludes(includeProperties);

            return await query.ToListAsync();
        }

        public async Task<List<T>> FindAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int skip = 0,
            int take = 0,            
            params string[] includeProperties)
        {
            if (skip < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip));
            }

            if (take < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(take));
            }

            IQueryable<T> query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip != 0)
            {
                query = query.Skip(skip);
            }

            if (take != 0)
            {
                query = query.Take(take);
            }
            
            query = query.AddIncludes(includeProperties);

            return await query.ToListAsync();
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();            
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();       
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbSet.AddRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

             _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> IncrementField(Expression<Func<T, bool>> filter, Expression<Func<T, int>> field, int quantity = 1)
        {
            throw new NotImplementedException();
        }
    }
}
