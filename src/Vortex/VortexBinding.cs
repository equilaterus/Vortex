using Equilaterus.Vortex.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex
{
    public abstract class VortexBinding
    {
        public List<Type> Actions { get; set; }

        public Type ReturnAction { get; set; }

        public bool ApplyLowerPriorityActions { get; set; } = false;

        public int Priority { get; set; } = 0;
    }
}
