using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Services
{
    public interface IRelationalDataStorage<T> : IDataStorage<T> where T : class
    {
        Task<List<T>> FindAllAsync(
            params string[] includeProperties);

        Task<List<T>> FindAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int skip = 0,
            int take = 0,            
            params string[] includeProperties);        
    }
}
