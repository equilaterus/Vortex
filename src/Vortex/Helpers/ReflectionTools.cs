using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Equilaterus.Vortex.Helpers
{
    public static class ReflectionTools
    {
        /// <summary>
        /// Instantiates a Type with 0 or N generic parameters.
        /// NOTES:
        /// - args must correspond with a constructor overload of TType.
        /// - Derived types with less Generic parameters are taken from left to right. 
        ///   Ex: Query(T) : ReturnAction(T,R)
        /// </summary>
        /// <typeparam name="TReturn">Expected return type. Cannot contain undefined generic parameters.</typeparam>
        /// <param name="type">Type to instantiate</param>
        /// <param name="args">Contructor args</param>
        /// <returns>Instance of TType</returns>
        public static TReturn InstantiateAs<TReturn>(Type type, params object[] args) where TReturn : class
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.ContainsGenericParameters)
            {
                type = type.MakeGenericType(
                    typeof(TReturn).GenericTypeArguments
                        .Take(type.GetGenericTypeDefinition().GetGenericArguments().Count())
                        .ToArray());
            }

            return Activator.CreateInstance(type, args) as TReturn;
        }

        /// <summary>
        /// Instantiates a Type with 0 or N generic parameters.
        /// NOTES:
        /// - args must correspond with a constructor overload of TType.
        /// - Derived types with less Generic parameters are taken from left to right. 
        ///   Ex: Query(T) : ReturnAction(T,R)
        /// </summary>
        /// <typeparam name="TType">Expected return type. Cannot contain undefined generic parameters.</typeparam>
        /// <param name="args">Contructor args</param>
        /// <returns>Instance of TType</returns>
        public static TType Instantiate<TType>(params object[] args) where TType : class
        {
            return InstantiateAs<TType>(typeof(TType), args);
        }
    }
}
