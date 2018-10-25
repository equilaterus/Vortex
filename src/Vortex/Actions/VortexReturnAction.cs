using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Actions
{
    public abstract class VortexReturnAction<TInput, TReturn>
    {
        public IVortexContext<TReturn> Context { get; private set; }

        public VortexReturnAction(IVortexContext<TReturn> context)
        {
            Context = context;
        }

        public abstract Task<TReturn> Execute(TInput input);
    }
}
