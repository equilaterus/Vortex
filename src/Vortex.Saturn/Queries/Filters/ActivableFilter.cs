using Equilaterus.Vortex.Helpers;
using Equilaterus.Vortex.Saturn.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.Saturn.Queries.Filters
{
    public class ActivableFilter<T> : QueryFilter<T> where T : class, IActivable
    {
        public override void UpdateParams(QueryParams<T> queryParams)
        {
            var condition = NewCondition(e => e.IsActive);
            queryParams.Filter = ExtendedExpression.Bind(queryParams.Filter, condition);
        }
    }
}
