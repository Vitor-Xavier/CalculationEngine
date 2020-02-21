

using System;

public class GenericValueLanguage
    {
        public readonly static GenericValueLanguage VOID = new GenericValueLanguage(new Object());

        public readonly Object Value;
        public readonly bool Contant;

        public GenericValueLanguage(Object value, bool constat = false)
        {
            Value = value;
            Contant = constat;
        }

        public Boolean AsBoolean()
        {
            return Boolean.Parse(Value.ToString());
        }

        public Double AsDouble()
        {
            return Double.Parse(Value.ToString());
        }

        public DateTime AsDateTime()
        {
            return DateTime.Parse(Value.ToString());
        }

        public static explicit operator double(GenericValueLanguage GenericValueLanguage)
        {
            if (GenericValueLanguage == null) return 0;
            double decimalConvert;
            double.TryParse(GenericValueLanguage.Value.ToString(), out decimalConvert);
            return decimalConvert;

        }
        public static explicit operator int(GenericValueLanguage GenericValueLanguage)
        {
            int intConvert;
            int.TryParse(GenericValueLanguage.Value.ToString(), out intConvert);
            return intConvert;
        }

        public String AsString()
        {
            return Value.ToString();
        }

        public bool IsDouble()
        {
            return Value is Double;
        }

        public bool IsDate()
        {
            return Value is DateTime;
        }

        public override int GetHashCode()
        {
            return Value == null ? 0 : Value.GetHashCode();
        }

        public override bool Equals(Object o)
        {
            if (Value == o)
            {
                return true;
            }

            if (Value == null || o == null)
            {
                return false;
            }

            var that = (GenericValueLanguage)o;

            return Value.Equals(that.Value);
        }

        public override String ToString()
        {
            if (IsDouble())
            {
                return Value.ToString().Replace(',', '.');
            }

            return Value.ToString();
        }
    }

