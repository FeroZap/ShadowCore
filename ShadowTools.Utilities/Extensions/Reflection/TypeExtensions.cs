using System;

namespace ShadowTools.Utilities.Extensions.Reflection
{
    public static class TypeExtensions
    {
        public static bool IsSimple(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || type == typeof(decimal)
                   || type == typeof(Guid)
                   || type == typeof(DateTime);
        }

        public static bool IsAssignableTo<T>(this Type type) where T: class
        {
            return typeof(T).IsAssignableFrom(type);
        }
    }
}
