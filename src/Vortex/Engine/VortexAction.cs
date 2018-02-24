using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Equilaterus.Vortex.Engine
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
            Priority = IsDefaultAction ? -1 : 0;            
        }

        public abstract Task Execute();        
    }

    public abstract class GenericAction<T> : VortexAction where T : class
    {
        public VortexContext<T> Context { get; protected set; }

        public GenericAction(VortexContext<T> context)
        {
            Context = context;
        }
    }
}
