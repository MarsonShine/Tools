using System;
using System.Collections;
using System.Reflection;

namespace FakeApi {
    public class FakeGenerator {
        public void Analyze<T>() where T : new() {
            var t = typeof(T);
            PrintRootTypeInfo<T>();
        }

        public void PrintRootTypeInfo<T>() {
            // 判断根类型是否是数组、可空类型、集合等
            var t = typeof(T);
            var rootObjTypeCode = Type.GetTypeCode(t);
            if (t.IsNullable()) {
                var elemType = t.GetGenericArguments() [0];
                // PrintTypeInfo(elemType);
            } else if (t.IsList()) {
                var genericType = t.GetGenericArguments();
                Activator.CreateInstance(t.Assembly.FullName, t.FullName);
            }
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties) {
                var propType = prop.PropertyType;
                var typeCode = Type.GetTypeCode(propType);
                var isListType = typeof(IList).IsAssignableFrom(propType);
                // 判断是否可空类型
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                    var nullableType = propType.GetGenericArguments() [0];
                    PrintNullableTypeInfo(nullableType, prop);
                } // 集合
                else if (isListType) {
                    if (propType.IsGenericType) {
                        // 获取集合类型
                        var genericType = propType.GetGenericArguments();
                        Console.WriteLine($"{prop.Name} -- 范型类型 {genericType[0].Name}");
                        PrintTypeInfo(genericType[0], prop);
                    } else if (propType.IsArray) {
                        var elemType = propType.GetElementType();
                        Console.WriteLine($"{prop.Name} -- 数组类型 {propType.Name}");
                        PrintTypeInfo(elemType, prop);
                    }
                } else if (propType.IsClass && typeCode == TypeCode.Object) {
                    PrintTypeInfo(propType, prop);
                } else if (propType.IsEnum) {
                    Console.WriteLine($"{propType.Name} -- 类型 Enum");
                } else {
                    PrintTypeInfoByCode(prop, typeCode);
                }
            }
        }

        private void PrintTypeInfo(Type t, PropertyInfo prop) {
            var typeCode = Type.GetTypeCode(t);
            // 判断是否可空类型
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                PrintNullableTypeInfo(t.GetGenericArguments() [0], prop);
            }
            // 集合 
            else if (typeof(IList).IsAssignableFrom(t)) {
                if (t.IsGenericType) {
                    // 获取集合类型
                    var genericType = t.GetGenericArguments();
                    Console.WriteLine($"{prop.Name} -- 范型类型 {genericType[0].Name}");
                    PrintTypeInfo(genericType[0], prop);
                } else if (t.IsArray) {
                    var elemType = t.GetElementType();
                    var props = elemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var p in props) {
                        PrintTypeInfo(p.PropertyType, p);
                    }
                }
            } else if (t.IsClass && typeCode == TypeCode.Object) {
                var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var p in props) {
                    PrintTypeInfo(p.PropertyType, p);
                }
            } else if (t.IsEnum) {
                Console.WriteLine($"{t.Name} -- 类型 Enum");
            } else {
                PrintTypeInfoByCode(prop, typeCode);
            }
        }

        private void PrintNullableTypeInfo(Type type, PropertyInfo prop) {
            Console.WriteLine($"{prop.Name} -- 可控类型 {type.Name}");
        }

        private void PrintTypeInfoByCode(PropertyInfo prop, TypeCode typeCode) {
            switch (typeCode) {
                case TypeCode.Boolean:
                    PrintTypeCode(prop, typeCode);
                    break;
                case TypeCode.String:
                    PrintTypeCode(prop, typeCode);
                    break;
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Int16:
                    PrintTypeCode(prop, typeCode);
                    break;
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    PrintTypeCode(prop, typeCode);
                    break;
                case TypeCode.Char:
                    PrintTypeCode(prop, typeCode);
                    break;
                case TypeCode.DateTime:
                    PrintTypeCode(prop, typeCode);
                    break;
                case TypeCode.Double:
                case TypeCode.Decimal:
                    PrintTypeCode(prop, typeCode);
                    break;
                default:
                    Console.WriteLine(prop.Name + " 未知属性");
                    break;
            }
        }

        private void PrintTypeCode(PropertyInfo prop, TypeCode typeCode) {
            Console.WriteLine($"{prop.Name} -- 类型 {typeCode.ToString()}");
        }

    }
}