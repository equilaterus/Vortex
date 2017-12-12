using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Behavior
{
    public interface IBaseBehavior<T> where T : IBaseModel<T>
    {
        Task<T> GetByIdAsync(T id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteByIdAsync(int id);
    }
}
