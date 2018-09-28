using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex
{
    public abstract class VortexAction
    {
        public VortexData Params { get; set; }

        public VortexData Results { get; protected set; }

        public bool PreventDefault { get; protected set; }

        public bool IsDefaultAction { get; protected set; }

        public bool IsReturnAction { get; protected set; }

        public int Priority { get; protected set; }        

        public virtual void Initialize()
        {
            if (Priority == 0)
                Priority = IsDefaultAction ? -1 : 0;            
        }

        public abstract Task Execute();        
    }

    public abstract class GenericAction<T> : VortexAction where T : class
    {
        public IVortexContext<T> Context { get; protected set; }

        public GenericAction(IVortexContext<T> context)
        {
            Context = context;
        }

        public GenericAction()
        {

        }
    }
}
