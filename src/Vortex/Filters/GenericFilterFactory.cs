using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Equilaterus.Vortex.Filters
{
    public class GenericFilterFactory : IGenericFilterFactory
    {
        public GenericFilterFactory()
        {
            _bindings = new Dictionary<Type, List<Type>>();
        }

        /// <summary>
        /// Binds a Type of Model Interface with its filter implementation.
        /// </summary>
        protected Dictionary<Type, List<Type>> _bindings;

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

            if (!_bindings.ContainsKey(modelInterface))
            {
                _bindings.Add(modelInterface, new List<Type>());
            }

            if (_bindings[modelInterface].Contains(filterImplementation))
            {
                throw new Exception("Binding already exists");
            }
            _bindings[modelInterface].Add(filterImplementation);
        }

        public List<QueryFilter<T>> GetFilters<T>() where T : class
        {
            var filters = new List<QueryFilter<T>>();

            var ttype = typeof(T);
            foreach (var binding in _bindings)
            {
                if (binding.Key.IsAssignableFrom(ttype))
                {
                    foreach (var implementation in binding.Value)
                    {
                        Type[] typeArgs = { typeof(T) };
                        var genericType = implementation.MakeGenericType(typeArgs);
                        var instance = Activator.CreateInstance(genericType);
                        filters.Add((QueryFilter<T>)instance);
                    }
                }
            }

            return filters;
        }
    }
}
