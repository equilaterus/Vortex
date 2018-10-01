using Equilaterus.Vortex.Saturn.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn
{
    public static class ExtendedPersistanceManager
    {
        public static async Task InsertEntityAsync<T>(this IPersistanceManager<T> p, T entity) where T : class
        {
            await p.ExecuteCommandAsync(
                VortexEvents.InsertEntity, new VortexData(entity));
        }

        public static async Task UpdateEntityAsync<T>(this IPersistanceManager<T> p, T entity) where T : class
        {
            await p.ExecuteCommandAsync(
                VortexEvents.UpdateEntity, new VortexData(entity));
        }

        public static async Task DeleteEntityAsync<T>(this IPersistanceManager<T> p, T entity) where T : class
        {
            await p.ExecuteCommandAsync(
                VortexEvents.DeleteEntity, new VortexData(entity));
        }

        public static async Task<List<T>> FindAllAsync<T>(this IPersistanceManager<T> p) where T : class
        {
            return await p.ExecuteQueryForEntitiesAsync(
                VortexEvents.QueryForEntities, new VortexData(new QueryParams<T>()));
        }

        public static async Task<List<T>> FindAsync<T>(this IPersistanceManager<T> p,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int skip = 0,
            int take = 0,
            bool skipFilters = false) where T : class
        {
            return await p.ExecuteQueryForEntitiesAsync(
                VortexEvents.QueryForEntities, 
                new VortexData(
                    new QueryParams<T>()
                    {
                        Filter = filter,
                        OrderBy = orderBy,
                        Skip = skip,
                        Take = take,
                        SkipFilters = skipFilters
                    }
                )
            );
        }

        public static async Task<int> CountAsync<T>(this IPersistanceManager<T> p,
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
