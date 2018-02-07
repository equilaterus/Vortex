using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.VortexGraph.Queries.Filters
{
    public interface IFilterFactory<T> where T : class
    {
        List<IQueryFilter<T>> GetFilters();
    }
}
