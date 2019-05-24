using System;

namespace Enumeration.Test
{
    public partial class UnitTest1
    {
        public class Calculation : Enumeration<Calculation, int>
        {
            public static readonly Calculation Add = new Calculation(1, "Add", (left, right) => left + right);
            public static readonly Calculation Subtract = new Calculation(2, "Subtract", (left, right) => left - right);
            public static readonly Calculation Multiply = new Calculation(3, "Multiply", (left, right) => left * right);

            private Calculation(int value, string displayName, Func<int, int, int> calculation)
                : base(value, displayName)
            {
                Go = calculation;
            }

            public Func<int, int, int> Go { get; private set; }
        }
    }
}
