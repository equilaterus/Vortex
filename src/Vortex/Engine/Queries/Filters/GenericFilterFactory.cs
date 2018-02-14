using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.Engine.Queries.Filters
{
    public class GenericFilterFactory<T> : IFilterFactory<T> where T : class
    {
        public List<IQueryFilter<T>> GetFilters()
        {
            var filters = new List<IQueryFilter<T>>();
            if (typeof(IActivable).IsAssignableFrom(typeof(T)))
            {
                var filter = typeof(ActivableFilter<>);
                Type[] typeArgs = { typeof(T) };
                var genericType = filter.MakeGenericType(typeArgs);
                var instance = Activator.CreateInstance(genericType);
                filters.Add((IQueryFilter<T>) instance);
            }
            if (typeof(ISoftDeleteable).IsAssignableFrom(typeof(T)))
            {
                var filter = typeof(SoftDeleteableFilter<>);
                Type[] typeArgs = { typeof(T) };
                var genericType = filter.MakeGenericType(typeArgs);
                var instance = Activator.CreateInstance(genericType);
                filters.Add((IQueryFilter<T>)instance);
            }
            return filters;
        }        
    }
}
