using Equilaterus.Vortex.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public class CrudBehavior<T> : ICrudBehavior<T> where T : class
    {
        protected readonly IPersistanceManager<T> _persistanceManager;

        public CrudBehavior(IPersistanceManager<T> persistanceManager)
        {
            _persistanceManager = persistanceManager;
        }
        
        public virtual async Task DeleteAsync(T entity)
        {
            await _persistanceManager.DeleteEntityAsync(entity);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<List<T>> FindAllAsync()
        {
            return await _persistanceManager.FindAllAsync();
        }

        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            int skip = 0, 
            int take = 0,
            bool skipFilters = false)
        {
            return await _persistanceManager.FindAsync(filter, orderBy, skip, take, skipFilters);
        }
         
        public virtual async Task InsertAsync(T entity)
        {
            await _persistanceManager.InsertEntityAsync(entity);
        }

        public virtual async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await _persistanceManager.UpdateEntityAsync(entity);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
