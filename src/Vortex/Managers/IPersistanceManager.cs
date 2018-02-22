using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Managers
{
    public interface IPersistanceManager<T>
    {
        Task<List<T>> ExecuteQuery(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int skip = 0,
            int take = 0);

        Task ExecuteCommand(string vortexEvent, T entity);
    }
}
