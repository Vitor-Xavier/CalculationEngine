using Common.Extensions;
using System;
using System.Collections.Generic;

namespace Implementation
{
    public readonly struct GenericValueLanguage
    {
        public readonly static GenericValueLanguage VOID = new GenericValueLanguage(new object());

        public readonly static GenericValueLanguage NULL = new GenericValueLanguage(null);

        public readonly static GenericValueLanguage Empty = new GenericValueLanguage(0);

        public readonly object Value;

        public bool IsNumeric => Value?.IsNumericType() ?? false;

        public GenericValueLanguage(object value) => Value = value;

        public static GenericValueLanguage operator +(GenericValueLanguage left, GenericValueLanguage right) =>
            new GenericValueLanguage(left.AsDecimal() + right.AsDecimal());

        public static GenericValueLanguage operator -(GenericValueLanguage left, GenericValueLanguage right) =>
            new GenericValueLanguage(left.AsDecimal() - right.AsDecimal());

        public static GenericValueLanguage operator *(GenericValueLanguage left, GenericValueLanguage right) =>
            new GenericValueLanguage(left.AsDecimal() * right.AsDecimal());

        public static GenericValueLanguage operator /(GenericValueLanguage left, GenericValueLanguage right) =>
            new GenericValueLanguage(left.AsDecimal() / right.AsDecimal());

        public static explicit operator decimal(GenericValueLanguage genericValue) =>
            decimal.Parse(genericValue.Value?.ToString() ?? "0");

        public static explicit operator int(GenericValueLanguage genericValue) =>
            int.Parse(genericValue.Value.ToString());

        public bool AsBoolean() => bool.Parse(Value.ToString());

        public decimal AsDecimal() => decimal.Parse(Value.ToString());

        public decimal TryDecimal() => decimal.TryParse(Value is decimal || Value is int || Value is string ? Value.ToString() : "0.0", out decimal valor) ? valor : 0;

        public int AsInt() => int.Parse(Value.ToString());

        public int AsInt(object Value) => int.Parse(Value.ToString());

        public DateTime AsDateTime() => DateTime.Parse(Value.ToString());

        public string AsString() => Value.ToString();

        public bool IsNull() => (Value is null);

        public IDictionary<int, IDictionary<string, GenericValueLanguage>> AsDictionaryIntDictionaryStringGeneric() => (Value as IDictionary<int, IDictionary<string, GenericValueLanguage>>);

        public IDictionary<int, GenericValueLanguage> AsDictionaryIntGeneric() => (Value as IDictionary<int, GenericValueLanguage>);

        public IDictionary<string, GenericValueLanguage> AsDictionaryStringGeneric() => (Value as IDictionary<string, GenericValueLanguage>);

        public IDictionary<string, GenericValueLanguage>[] AsDictionaryStringGenericArray() => (Value as IDictionary<string, GenericValueLanguage>[]);

        public bool IsGenericValueLanguage() => (Value is GenericValueLanguage);

        public bool IsDictionaryIntDictionaryStringGeneric() => (Value is IDictionary<int, IDictionary<string, GenericValueLanguage>>);

        public bool IsDictionaryStringGeneric() => (Value is IDictionary<string, GenericValueLanguage>);

        public bool IsDictionaryIntGeneric() => (Value is IDictionary<int, GenericValueLanguage>);

        public bool IsDictionaryStringGenericArray() => (Value is IDictionary<string, GenericValueLanguage>[]);

        public bool IsDecimal() => Value is decimal;

        public bool IsInt() => Value is int;

        public bool IsDate() => Value is DateTime;

        public override int GetHashCode() =>
            Value == null ? 0 : Value.GetHashCode();

        public override bool Equals(object o)
        {
            if (Value == o)
                return true;

            if (Value is null || o is null)
                return false;

            var that = (GenericValueLanguage)o;

            return Value.Equals(that.Value);
        }

        public override string ToString() =>
            IsDecimal() ? Value.ToString().Replace(',', '.') : Value?.ToString();
    }
}
