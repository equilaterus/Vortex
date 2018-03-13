using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Helpers
{
    public class SubClassOf<T> where T : class
    {
        public Type TypeOf { get; private set; }

        public static SubClassOf<T> GetFrom<Q>() where Q : T
        {
            return new SubClassOf<T>(typeof(Q));
        }

        public static SubClassOf<T> GetFrom(Type type)
        {
            if (!typeof(T).IsAssignableFrom(type))
            {
                throw new Exception($"{nameof(type)} is not a subclass of <T>");
            }

            return new SubClassOf<T>(type);
        }

        private SubClassOf(Type type)
        {
            TypeOf = type;
        }
    }
}
