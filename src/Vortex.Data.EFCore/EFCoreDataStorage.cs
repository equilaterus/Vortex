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
    public class EFCoreDataStorage<T> /*: IDataStorage<T>*/ where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public EFCoreDataStorage(DbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }

              

        public async Task<List<T>> FindAllAsync(string includeProperty)
        {
            IQueryable<T> query = _dbSet;
            //query.AddIncludes(includeProperties);
            return await query.ToListAsync();
        }

        private async Task<List<T>> FindAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int top = 0,
            params string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }        
               


        public async Task InsertAsync(T entity)
        {
            CheckEntity(entity);
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            CheckEntity(entity);
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            CheckEntity(entity);
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void CheckEntity(T entity)
        {
            if (entity == null)
            {
                throw new Exception("Null entity");
            }
        }

        public Task InsertRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<T> IncrementField(Expression<Func<T, bool>> filter, Expression<Func<T, int>> field, int quantity = 1)
        {
            throw new NotImplementedException();
        }
    }
}
