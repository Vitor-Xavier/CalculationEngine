using System;

    public class GenericValueLanguage
    {
        public readonly static GenericValueLanguage VOID = new GenericValueLanguage(new object());

        public readonly object Value;
        public readonly bool Contant;

        public GenericValueLanguage(object value, bool constat = false)
        {
            Value = value;
            Contant = constat;
        }

        public bool AsBoolean() => bool.Parse(Value.ToString());

        public double AsDouble() => double.Parse(Value.ToString());

        public DateTime AsDateTime() =>  DateTime.Parse(Value.ToString());

        public static explicit operator double(GenericValueLanguage GenericValueLanguage)
        {
            if (GenericValueLanguage == null) return 0;
            double.TryParse(GenericValueLanguage.Value.ToString(), out double decimalConvert);
            return decimalConvert;

        }
        public static explicit operator int(GenericValueLanguage GenericValueLanguage)
        {
            int.TryParse(GenericValueLanguage.Value.ToString(), out int intConvert);
            return intConvert;
        }

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

