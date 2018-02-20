using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine
{
    public class VortexData
    {
        public object Entity;

        public T GetMainEntityAs<T>() 
            where T : class
        {
            return Entity as T;
        }

        public VortexData(object entity)
        {
            Entity = entity;
        }
    }
}
