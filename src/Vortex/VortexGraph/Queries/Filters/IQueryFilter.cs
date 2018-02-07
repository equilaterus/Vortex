﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.VortexGraph.Queries.Filters
{
    public interface IQueryFilter<T> where T : class
    {
        void UpdateParams(QueryParams<T> queryParams);
    }
}
