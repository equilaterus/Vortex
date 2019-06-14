using System;
using System.Collections.Generic;
using System.Text;

namespace Equilaterus.Vortex.Helpers
{
    public static class ObjectExtensions
    {
        public static Maybe<T?> AsMaybe<T>(this T? obj) where T : struct
        {
            if (obj is null)
                return new Maybe<T?>();
            else
                return new Maybe<T?>((T)obj);
        }

        public static Maybe<T> AsMaybe<T>(this T obj) where T : class
        {
            if (obj is null)
                return new Maybe<T>();
            else
                return new Maybe<T>(obj);
        }
    }
}
