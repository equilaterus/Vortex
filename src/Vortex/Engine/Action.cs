using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine
{
    public abstract class Action
    {
        protected ActionParams _params;

        public bool ExcecuteDefaultActions;
                
        public void SetParams(ActionParams actionParams)
        {
            _params = actionParams;
        }

        public abstract void Excecute();
    }
}
