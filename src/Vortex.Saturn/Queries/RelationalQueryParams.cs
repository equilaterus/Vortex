using Equilaterus.Vortex.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Saturn.Queries
{
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
