using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Helpers
{
    public class SubClassOf<T> 
        where T : class        
    {
        public Type TypeOf { get; private set; }

        public void Initialize<Q>()
            where Q : T
        {
            TypeOf = typeof(Q);
        }

        public static SubClassOf<T> GetFrom<Q>()
            where Q : T
        {
            var subclass = new SubClassOf<T>();
            subclass.Initialize<Q>();
            return subclass;
        }

        private SubClassOf()
        {
        }
    }
}
