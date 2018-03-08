using Equilaterus.Vortex.Helpers;
using Equilaterus.Vortex.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.Engine.Queries.Filters
{
    public class ActivableFilter<T> : QueryFilter<T> where T : class, IActivable
    {
        public override void UpdateParams(QueryParams<T> queryParams)
        {
            var condition = NewCondition(e => e.IsActive);
            queryParams.Filter = queryParams.Filter.Bind(condition);
        }
    }
}
