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
    public class QueryForEntities<T> : VortexReturnAction<T, List<T>> where T : class
    {
        public override async Task<List<T>> Execute(QueryParams<T> queryParams, params object[] parameters)
        {
            var dataStorage = this.GetContext().DataStorage;

            return await dataStorage.FindAsync(queryParams);
        }

        public QueryForEntities(VortexContext<T> context, IGenericFilterFactory filterFactory)
            : base(context, filterFactory) { }
    }
}
