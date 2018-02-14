using Equilaterus.Vortex.Helpers;
using Equilaterus.Vortex.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.Engine.Queries.Filters
{
    public class SoftDeleteableFilter<T> : IQueryFilter<T> where T : class, ISoftDeleteable
    {
        public void UpdateParams(QueryParams<T> queryParams)
        {
            var inner = PredicateBuilder.New<T>();
            inner = inner.Start(e => e.IsDeleted == false);

            queryParams.Filter = inner.And(queryParams.Filter);
        }
    }
}
