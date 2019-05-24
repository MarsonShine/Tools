/// <summary>
/// copy from https://github.com/HeadspringLabs/Enumeration/blob/master/Enumeration.cs
/// </summary>
namespace Enumeration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;

    [Serializable]
    [DebuggerDisplay("{DisplayName} - {Value}")]
    public class Enumeration<TEnumeration> : Enumeration<TEnumeration, int>
        where TEnumeration : Enumeration<TEnumeration>
    {
        protected Enumeration(int value, string displayName) : base(value, displayName)
        {

        }
        public static TEnumeration FromInt32(int value)
        {
            return FromValue(value);
        }

        public static bool TryFromInt32(int listItemValue, out TEnumeration result)
        {
            return TryParse(listItemValue, out result);
        }
    }
    [Serializable]
    [DebuggerDisplay("{DisplayName} - {Value}")]
    [DataContract(Namespace = "http://github.com/HeadspringLabs/Enumeration")]
    public abstract class Enumeration<TEnumeration, TValue> : IComparable<TEnumeration>, IEquatable<TEnumeration>
        where TEnumeration : Enumeration<TEnumeration, TValue>
        where TValue : IComparable
    {
        private static readonly Lazy<TEnumeration[]> Enumerations = new Lazy<TEnumeration[]>(GetEnumerations);

        [DataMember(Order = 0)]
        private readonly TValue _value;
        [DataMember(Order = 1)]
        private readonly string _displayName;

        protected Enumeration(TValue value, string displayName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            this._value = value;
            this._displayName = displayName;
        }
        public TValue Value => _value;
        public string DisplayName => _displayName;
        public static TEnumeration[] GetAll() => Enumerations.Value;

        protected static TEnumeration FromValue(TValue value)
        {
            return Parse(value, "value", item => item.Value.Equals(value));
        }

        private static TEnumeration Parse(TValue value, string description, Func<TEnumeration, bool> predicate)
        {
            if (!TryParse(predicate, out TEnumeration result))
            {
                string message = $"'{value}' is not a valid {description} in {typeof(TEnumeration)}";
                throw new ArgumentException(message, "value");
            }
            return result;
        }
        public static bool TryParse(TValue value, out TEnumeration result)
        {
            return TryParse(e => e.ValueEquals(value), out result);
        }
        private static TEnumeration[] GetEnumerations()
        {
            Type enumerationType = typeof(TEnumeration);
            return enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
                .Select(info => info.GetValue(null))
                .Cast<TEnumeration>()
                .ToArray();
        }
        static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
        {
            result = GetAll().FirstOrDefault(predicate);
            return result != null;
        }

        public int CompareTo(TEnumeration other)
        {
            return Value.CompareTo(other == default(TEnumeration) ? default(TValue) : other.Value);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as TEnumeration);
        }

        public bool Equals(TEnumeration other)
        {
            return other != null && ValueEquals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            return Equals(left, right);
        }
        public static bool operator !=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            return !Equals(left, right);
        }

        protected virtual bool ValueEquals(TValue value)
        {
            return Value.Equals(value);
        }
        public override sealed string ToString()
        {
            return DisplayName;
        }
    }
}
