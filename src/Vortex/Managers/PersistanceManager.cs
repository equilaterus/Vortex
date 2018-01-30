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

        public async Task<List<T>> FindAllAsync(params string[] includeProperties)
        {
            // return await _dataStorage.FindAllAsync(includeProperties);
            return null;
        }

        public async Task<List<T>> FindAsync(
            Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            int skip = 0, 
            int take = 0, 
            params string[] includeProperties)
        {

            //return await _dataStorage.FindAsync(
            //filter, orderBy, skip, take, includeProperties);
            return null;
        }

        public async Task InsertAsync(T entity)
        {
            await _dataStorage.InsertAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await _dataStorage.InsertRangeAsync(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            await _dataStorage.UpdateAsync(entity);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            await _dataStorage.UpdateRangeAsync(entities);
        }

        public async Task DeleteAsync(T entity)
        {
            await _dataStorage.DeleteAsync(entity);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            await _dataStorage.DeleteRangeAsync(entities);
        }
        
        public async Task IncrementField(
            Expression<Func<T, bool>> filter, 
            Expression<Func<T, int>> field, 
            int quantity = 1)
        {
            //await _dataStorage.IncrementField(filter, field, quantity);
        }        
    }
}
