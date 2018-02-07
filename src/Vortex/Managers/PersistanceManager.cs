using Equilaterus.Vortex.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Managers
{
    public class PersistanceManager<T> : IPersistanceManager<T> where T : class
    {
        protected readonly IDataStorage<T> _dataStorage;

        public PersistanceManager(IDataStorage<T> dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public Task<List<T>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> FindAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int skip = 0, int take = 0)
        {
            throw new NotImplementedException();
        }


        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task InsertRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
