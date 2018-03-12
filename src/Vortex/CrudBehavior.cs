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
        protected IPersistanceManager<T> PersistanceManager { get; private set; }

        public virtual async Task DeleteAsync(T entity)
        {
            await PersistanceManager.DeleteEntityAsync(entity);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<List<T>> FindAllAsync()
        {
            return await PersistanceManager.FindAllAsync();
        }

        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            int skip = 0, 
            int take = 0)
        {
            return await PersistanceManager.FindAsync(filter, orderBy, skip, take);
        }
         
        public virtual async Task InsertAsync(T entity)
        {
            await PersistanceManager.InsertEntityAsync(entity);
        }

        public virtual async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await PersistanceManager.UpdateEntityAsync(entity);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
