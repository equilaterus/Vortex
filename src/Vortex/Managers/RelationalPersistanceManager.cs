using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Managers
{
    public static class RelationalPersistanceManager
    {
        public static async Task<List<T>> Find<T>(this PersistanceManager<T> p,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int skip = 0,
            int take = 0,
            params string[] includeProperties) where T : class
        {
            return await p.ExecuteQueryForEntitiesAsync(
                VortexEvents.RelationalQueryForEntities, 
                new VortexData(
                    new RelationalQueryParams<T>()
                    {
                        Filter = filter,
                        OrderBy = orderBy,
                        Skip = skip,
                        Take = take,
                        IncludeProperties = includeProperties
                    }
                )
            );
        }
    }
}
