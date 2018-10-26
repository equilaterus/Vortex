using System;
using System.Linq;
using System.Linq.Expressions;

namespace Equilaterus.Vortex.Filters
{
    public class QueryParams<T>
    {
        public Expression<Func<T, bool>> Filter { get; set; } = null;

        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; } = null;

        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 0;

        public bool SkipFilters { get; set; } = false;
    }
}
