using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.Engine.Queries.Filters
{
    public class GenericFilterFactory<T> where T : class
    {
        /// <summary>
        /// Binds a Type of Model Interface with its filter implementation.
        /// </summary>
        protected List<Tuple<Type, Type>> _bindings = new List<Tuple<Type, Type>>();

        public void Bind(Type modelInterface, Type filterImplementation)
        {
            _bindings.Add(new Tuple<Type, Type>(modelInterface, filterImplementation));
        }

        public List<QueryFilter<T>> GetFilters()
        {
            var filters = new List<QueryFilter<T>>();

            var ttype = typeof(T);
            foreach (var binding in _bindings)
            {
                if (binding.Item1.IsAssignableFrom(ttype))
                {
                    var filter = binding.Item2;
                    Type[] typeArgs = { typeof(T) };
                    var genericType = filter.MakeGenericType(typeArgs);
                    var instance = Activator.CreateInstance(genericType);
                    filters.Add((QueryFilter<T>)instance);
                }
            }

            return filters;
        }        
    }
}
