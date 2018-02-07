using Equilaterus.Vortex.Helpers;
using Equilaterus.Vortex.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.VortexGraph.Queries.Filters
{
    public class ActivableFilter<T> : IQueryFilter<T> where T : class, IActivable
    {
        public void UpdateParams(QueryParams<T> queryParams)
        {
            var inner = PredicateBuilder.New<T>();
            inner = inner.Start(e => e.IsActive);

            queryParams.Filter = inner.And(queryParams.Filter);
        }
    }
}
