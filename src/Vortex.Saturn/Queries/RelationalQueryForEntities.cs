using Equilaterus.Vortex.Saturn.Services;
using Equilaterus.Vortex.Saturn.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Filters;

namespace Equilaterus.Vortex.Saturn.Queries
{
    public class RelationalQueryForEntities<T> : VortexReturnAction<T, List<T>> where T : class
    {
        public override async Task<List<T>> Execute(QueryParams<T> queryParams)
        {
            var dataStorage = this.GetContext().DataStorage;

            if (queryParams is RelationalQueryParams<T> relationalQueryParams)
            {
                if (dataStorage is IRelationalDataStorage<T> relationalDataStorage)
                {
                    return await relationalDataStorage.FindAsync(relationalQueryParams);
                }
                else
                {
                    throw new Exception("Relational DataStorage not found. Ensure to be inyecting it.");
                }  
            }
            else
            {
                return await dataStorage.FindAsync(queryParams);
            }
        }

        public RelationalQueryForEntities(VortexContext<T> context, IGenericFilterFactory filterFactory)
            : base(context, filterFactory) { }
    }
}
