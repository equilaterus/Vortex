using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Services.EFCore
{
    public class EFCoreDataStorage<T> : IRelationalDataStorage<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public DbContext DbContext { get { return _dbContext; } }

        public EFCoreDataStorage(DbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }

        private void DetachAll(IEnumerable<T> entities)
        {            
            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }
        }

        public async Task<List<T>> FindAllAsync()
        {
            return await FindAllAsync(null);
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
            int take = 0)
        {
            return await FindAsync(filter, orderBy, skip, take, null);
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

        public async Task<int> Count(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();

            DetachAll(entities);
        }
        
        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbSet.AddRange(entities);
            await _dbContext.SaveChangesAsync();

            DetachAll(entities);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

             _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();

            DetachAll(entities);
        }        
    }
}
