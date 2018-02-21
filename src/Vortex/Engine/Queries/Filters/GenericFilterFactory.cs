using Equilaterus.Vortex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.Engine.Queries.Filters
{
    public class GenericFilterFactory
    {
        /// <summary>
        /// Binds a Type of Model Interface with its filter implementation.
        /// </summary>
        public Dictionary<Type, List<Type>> Bindings { get; protected set; }

        public void Bind(Type modelInterface, Type filterImplementation)
        {
            if (modelInterface == null)
            {
                throw new ArgumentNullException(nameof(modelInterface));
            }
            if (filterImplementation == null)
            {
                throw new ArgumentNullException(nameof(filterImplementation));
            }

            if (!Bindings.ContainsKey(modelInterface))
            {
                Bindings.Add(modelInterface, new List<Type>());
            }

            Bindings[modelInterface].Add(filterImplementation);
        }

        public List<QueryFilter<T>> GetFilters<T>() where T : class
        {
            var filters = new List<QueryFilter<T>>();

            var ttype = typeof(T);
            foreach (var binding in Bindings)
            {
                if (binding.Key.IsAssignableFrom(ttype))
                {
                    foreach (var implementation in binding.Value)
                    {
                        var filter = implementation;
                        Type[] typeArgs = { typeof(T) };
                        var genericType = filter.MakeGenericType(typeArgs);
                        var instance = Activator.CreateInstance(genericType);
                        filters.Add((QueryFilter<T>)instance);
                    }
                }
            }

            return filters;
        }

        public GenericFilterFactory()
        {
            Bindings = new Dictionary<Type, List<Type>>();
        }
    }
}
