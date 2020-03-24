namespace EnumConvert
{
    public class EnumDescription
    {
        public EnumDescription(string name, string value, string description)
        {
            Name = name;
            Value = value;
            Description = description;
        }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}