using System;

    public class GenericValueLanguage
    {
        public readonly static GenericValueLanguage VOID = new GenericValueLanguage(new object());

        public readonly object Value;

        public GenericValueLanguage(object value)
        {
            Value = value;
        }

        public bool AsBoolean() => bool.Parse(Value.ToString());

        public double AsDouble() => double.Parse(Value.ToString());

        public DateTime AsDateTime() =>  DateTime.Parse(Value.ToString());

        public string AsString() => Value.ToString();

        public bool IsDouble() => Value is double;

        public bool IsDate() => Value is DateTime;

        public override int GetHashCode() =>
            Value == null ? 0 : Value.GetHashCode();

        public override bool Equals(object o)
        {
            if (Value == o)
            {
                return true;
            }

            if (Value is null || o is null)
            {
                return false;
            }

            var that = (GenericValueLanguage)o;

            return Value.Equals(that.Value);
        }

        public override string ToString() =>
            IsDouble() ? Value.ToString().Replace(',', '.') : Value.ToString();
    }

