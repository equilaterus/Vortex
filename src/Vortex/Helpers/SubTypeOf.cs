using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Helpers
{
    public class SubTypeOf<T> where T : class
    {
        public Type TypeOf { get; private set; }

        public static SubTypeOf<T> GetFrom<Q>() where Q : T
        {
            return new SubTypeOf<T>(typeof(Q));
        }

        public static SubTypeOf<T> GetFrom(Type type)
        {
            if (!typeof(T).IsAssignableFrom(type))
            {
                throw new Exception($"{nameof(type)} is not a subclass of <T>");
            }

            return new SubTypeOf<T>(type);
        }

        private SubTypeOf(Type type)
        {
            TypeOf = type;
        }
    }
}
