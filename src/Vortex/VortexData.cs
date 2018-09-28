using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex
{
    public class VortexData
    {
        public object Entity { get; protected set; }

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
