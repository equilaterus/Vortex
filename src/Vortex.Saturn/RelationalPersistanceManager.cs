using Equilaterus.Vortex.Saturn.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn
{
    public static class RelationalPersistanceManager
    {
        public static async Task<List<T>> FindAsync<T>(this IPersistanceManager<T> p,
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

        public static async Task<List<T>> FindAllAsync<T>(this IPersistanceManager<T> p,
            params string[] includeProperties) where T : class
        {
            return await p.FindAsync(null, null, 0, 0, includeProperties);
        }
    }
}
