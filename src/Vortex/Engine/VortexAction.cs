using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine
{
    public abstract class VortexAction
    {
        protected ActionParams _params;

        public bool ExcecuteDefaultActions { get; protected set; }

        public bool IsDefaultAction { get; protected set; }

        public int Priority { get; protected set; }

        public void SetParams(ActionParams actionParams)
        {
            _params = actionParams;
        }

        public virtual void Initialize()
        {
            ExcecuteDefaultActions = true;
            IsDefaultAction = false;
            Priority = 0;
        }

        public abstract void Excecute();        
    }
}
