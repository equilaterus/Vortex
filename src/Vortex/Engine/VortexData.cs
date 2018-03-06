using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine
{
    public class VortexData
    {
        public object Entity { get; protected set; }

        public T GetMainEntityAs<T>() 
            where T : class
        {
            return Entity as T;
        }

        public int GetAsInt()
        {
            return (int)Entity;
        }

        public VortexData(object entity)
        {
            Entity = entity;
        }
    }
}
