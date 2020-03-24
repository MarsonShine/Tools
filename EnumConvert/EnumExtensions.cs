using System;
using System.ComponentModel;

namespace EnumConvert
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum @enum)
        {
            var fieldInfo = @enum.GetType().GetField(@enum.ToString());
            var attrs = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs?.Length > 0) return attrs[0].Description;
            return @enum.ToString();
        }
    }
}