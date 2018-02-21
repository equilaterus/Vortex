using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.Engine.Queries.Filters
{
    public abstract class QueryFilter<T> where T : class
    {
        protected ExpressionStarter<T> NewCondition(Expression<Func<T, bool>> expression)
        {
            var condition = PredicateBuilder.New<T>();
            return condition.Start(expression);
        }

        public abstract void UpdateParams(QueryParams<T> queryParams); 
    }
}
