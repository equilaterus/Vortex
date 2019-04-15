using System;
using System.Reflection;

namespace Vortex.Tests.Utils
{
    public static class ReflectionExtended
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
