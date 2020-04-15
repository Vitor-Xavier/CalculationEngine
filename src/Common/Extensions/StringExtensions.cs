using System.ComponentModel;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static string SubstringWithIndexOf(this string str, char caracter, bool isFinal = false)
        {
            int position = str.IndexOf(caracter);

            if (position < 0)
                return str;

            return isFinal ? str[^(str.Length - position)..].Trim() : str[..position].Trim();
        }

        public static string RemoveCaracter(this string str, string caracter) =>
            str.Replace(caracter, string.Empty).Trim();

        public static T? ToNullable<T>(this string s) where T : struct
        {
            T? result = new T?();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;
        }

    }
}