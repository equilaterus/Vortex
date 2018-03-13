using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.Engine.Queries
{
    public class QueryParams<T>
    {
        public Expression<Func<T, bool>> Filter { get; set; } = null;

        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; } = null;

        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 0;

        public bool SkipFilters { get; set; } = false;
    }

    public class RelationalQueryParams<T> : QueryParams<T>
    {
        public string[] IncludeProperties { get; set; } = null;

        public RelationalQueryParams() { }

        public RelationalQueryParams(QueryParams<T> q)
        {
            Filter = q.Filter;

            OrderBy = q.OrderBy;

            Skip = q.Skip;

            Take = q.Take;

            SkipFilters = q.SkipFilters;
        }
    }
}
