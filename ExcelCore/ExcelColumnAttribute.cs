using System;

namespace ExcelCore
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ExcelColumnAttribute : Attribute
    {
        public ExcelColumnAttribute() { }
        public ExcelColumnAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public int Order { get; set; }
        public bool Ignore { get; set; }
        public string FontColor { get; set; }
    }
}