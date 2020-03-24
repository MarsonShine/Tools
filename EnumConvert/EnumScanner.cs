using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace EnumConvert
{
    public class EnumScanner
    {
        public Dictionary<string, List<EnumDescription>> ToDescription(Type enumType)
        {
            var dic = new Dictionary<string, List<EnumDescription>>();
            if (typeof(Enum).IsAssignableFrom(enumType))
            {
                var fields = enumType.GetFields();
                var descs = new List<EnumDescription>(fields.Length);
                foreach (var field in fields)
                {
                    var attr = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));
                    if (attr != null)
                    {
                        var desc = new EnumDescription(field.GetValue(null)?.ToString(), ((int)field.GetValue(null)).ToString(), attr.Description);

                        descs.Add(desc);
                    }
                }
                dic.Add(enumType.Name, descs);
                return dic;
            }
            else
            {
                return default;
            }
        }

        public List<Dictionary<string, List<EnumDescription>>> Scan<T>() where T : Enum
        {
            var enumType = typeof(T);
            var enumTypeAssembly = enumType.Assembly;
            var enumTypeAssemblies = enumTypeAssembly.GetTypes()
            .Where(p => typeof(Enum).IsAssignableFrom(p))
            .ToList();

            var dics = new List<Dictionary<string, List<EnumDescription>>>();
            foreach (var item in enumTypeAssemblies)
            {
                var dicItem = ToDescription(item);
                dics.Add(dicItem);
            }

            return dics;
        }
    }
}