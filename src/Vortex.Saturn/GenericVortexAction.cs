using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Saturn
{
    public abstract class GenericAction<T> : VortexAction where T : class
    {
        public VortexContext<T> Context { get; protected set; }

        public GenericAction(VortexContext<T> context)
        {
            Context = context;
        }

        public GenericAction()
        {

        }
    }
}
