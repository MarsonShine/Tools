using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExcelCore
{
    public class ReflectionHelper
    {
        public static List<PropertyInfo> GetSpecificalProps(object obj, Type type)
        {
            if (obj == null) throw new ArgumentException(nameof(obj));

            var props = obj.GetType().GetTypeInfo()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // 获取指定类型的属性
            var specificalPropType = props.Where(p => type.IsAssignableFrom(p.PropertyType))
                .ToList();

            return specificalPropType;
        }

        public static List<PropertyInfo> GetProperties(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var props = obj.GetType().GetTypeInfo()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // 获取指定类型的属性

            return props.ToList();
        }
    }
}