﻿using System;
using System.Collections.Generic;

namespace Equilaterus.Vortex.Filters
{
    public interface IGenericFilterFactory
    {        
        void Bind(Type modelInterface, Type filterImplementation);

        List<QueryFilter<T>> GetFilters<T>() where T : class;
    }
}