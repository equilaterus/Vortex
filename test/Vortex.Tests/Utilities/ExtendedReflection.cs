using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Vortex.Tests.Utilities
{
    public static class ExtendedReflection
    {
        public static T GetPrivateField<T>(this object obj, string fieldName)
        {
            if (fieldName == null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            return (T)obj.GetType()
                .GetField(
                    fieldName,
                    BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(obj);
        }
    }
}
