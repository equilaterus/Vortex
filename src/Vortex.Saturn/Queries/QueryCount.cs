using Equilaterus.Vortex.Actions;
using Equilaterus.Vortex.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Saturn.Queries
{
    public class QueryCount<T> : VortexReturnAction<T, int> where T : class
    {
        public override async Task<int> Execute(QueryParams<T> queryParams)
        {
            var dataStorage = this.GetContext().DataStorage;

            return await dataStorage.Count(queryParams.Filter);
        }

        public QueryCount(VortexContext<T> context, IGenericFilterFactory filterFactory) 
            : base(context, filterFactory) { }
    }
}
