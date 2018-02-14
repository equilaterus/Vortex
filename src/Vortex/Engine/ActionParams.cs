using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Engine
{
    public class ActionParams
    {
        public object Entity;

        public T GetMainEntityAs<T>() 
            where T : class
        {
            return Entity as T;
        }

        public ActionParams(object entity)
        {
            Entity = entity;
        }
    }
}
