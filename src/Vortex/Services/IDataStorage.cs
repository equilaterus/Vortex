﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services
{
    public interface IDataStorage<T> where T : class
    {
        Task<List<T>> FindAllAsync(
            params string[] includeProperties);
        
        Task<List<T>> FindAsync(            
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int top = 0,
            params string[] includeProperties);        

        Task InsertAsync(T entity);

        Task InsertRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task UpdateRangeAsync(IEnumerable<T> entities);

        Task DeleteAsync(T entity);

        Task DeleteRangeAsync(IEnumerable<T> entities);

        Task<T> IncrementField(
            Expression<Func<T, bool>> filter, 
            Expression<Func<T, int>> field, 
            int quantity = 1);
    }
}
