using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Actions
{
    public abstract class VortexAction<T>
    {
        public IVortexContext<T> Context { get; private set; }

        public VortexAction(IVortexContext<T> context)
        {
            Context = context;
        }

        public abstract Task Execute(T entity, params object[] parameters);
    }
}
