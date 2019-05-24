namespace Enumeration.Test
{
    public partial class UnitTest1
    {
        private class Color : Enumeration<Color, int>
        {
            public static readonly Color Red = new Color(1, "Red");
            public static readonly Color Blue = new Color(2, "Blue");
            public static readonly Color Green = new Color(3, "Green");
            public Color(int value, string displayName) : base(value, displayName)
            {
            }
        }
    }
}
