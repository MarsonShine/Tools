using System;
using System.Collections;

namespace FakeApi {
    public static class TypeExtensions {
        public static bool IsNullable(this Type type) {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsList(this Type type) {
            return typeof(IList).IsAssignableFrom(type) && type.IsGenericType;
        }
    }
}