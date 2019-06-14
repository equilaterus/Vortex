using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Helpers
{
    public static class ObjectExtensions
    {
        public static Maybe<T> AsMaybe<T>(this T obj)
        {
            if (obj is null)
                return new Maybe<T>();
            else
                return new Maybe<T>(obj);
        }
    }
}
