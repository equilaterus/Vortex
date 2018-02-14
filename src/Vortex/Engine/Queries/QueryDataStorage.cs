using Equilaterus.Vortex.Services;
using Equilaterus.Vortex.Engine.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine.Queries
{
    public static class QueryDataStorage
    {
        public static async Task<List<T>> FindAsync<T>(this IDataStorage<T> ds, QueryParams<T> queryParams) where T : class
        {
            if (queryParams == null)
            {
                return await ds.FindAllAsync();
            }
            else
            {
                return await ds.FindAsync(
                    queryParams.Filter,
                    queryParams.OrderBy,
                    queryParams.Skip,
                    queryParams.Take);
            }
        }

        public static async Task<List<T>> FindAsync<T>(this IRelationalDataStorage<T> ds, RelationalQueryParams<T> queryParams) where T : class
        {
            if (queryParams == null)
            {
                return await ds.FindAllAsync();
            }
            else
            {
                return await ds.FindAsync(
                    queryParams.Filter,
                    queryParams.OrderBy,
                    queryParams.Skip,
                    queryParams.Take,
                    queryParams.IncludeProperties);
            }
        }
    }
}
