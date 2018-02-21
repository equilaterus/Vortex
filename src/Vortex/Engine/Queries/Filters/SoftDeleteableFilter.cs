using Equilaterus.Vortex.Helpers;
using Equilaterus.Vortex.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.Engine.Queries.Filters
{
    public class SoftDeleteableFilter<T> : QueryFilter<T> where T : class, ISoftDeleteable
    {
        public override void UpdateParams(QueryParams<T> queryParams)
        {
            var condition = NewCondition(e => e.IsDeleted == false);

            queryParams.Filter = condition.And(queryParams.Filter);
        }
    }
}
