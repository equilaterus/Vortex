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
    public static class ExtendedPersistanceManager
    {
        public static async Task InsertEntity<T>(this IPersistanceManager<T> p, T entity) where T : class
        {
            await p.ExecuteCommandAsync(
                VortexEvents.InsertEntity, new VortexData(entity));
        }

        public static async Task UpdateEntity<T>(this IPersistanceManager<T> p, T entity) where T : class
        {
            await p.ExecuteCommandAsync(
                VortexEvents.UpdateEntity, new VortexData(entity));
        }

        public static async Task DeleteEntity<T>(this IPersistanceManager<T> p, T entity) where T : class
        {
            await p.ExecuteCommandAsync(
                VortexEvents.DeleteEntity, new VortexData(entity));
        }

        public static async Task<List<T>> FindAll<T>(this IPersistanceManager<T> p) where T : class
        {
            return await p.ExecuteQueryForEntitiesAsync(
                VortexEvents.QueryForEntities, new VortexData(new QueryParams<T>()));
        }

        public static async Task<List<T>> Find<T>(this IPersistanceManager<T> p,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int skip = 0,
            int take = 0) where T : class
        {
            return await p.ExecuteQueryForEntitiesAsync(
                VortexEvents.QueryForEntities, 
                new VortexData(
                    new QueryParams<T>()
                    {
                        Filter = filter,
                        OrderBy = orderBy,
                        Skip = skip,
                        Take = take
                    }
                )
            );
        }

        public static async Task<int> Count<T>(this IPersistanceManager<T> p,
            Expression<Func<T, bool>> filter = null) where T : class
        {
            return await p.ExecuteQueryForIntAsync(
                VortexEvents.QueryCount,
                new VortexData(
                    new QueryParams<T>()
                    {
                        Filter = filter
                    }
                )
            );
        }
    }
}
