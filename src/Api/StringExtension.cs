using System;

namespace Api
{
    public static class StringExtension {

        public static string SubstringWithIndexOf (this String str, char caracter, bool isFinal = false) {

            int position = str.IndexOf (caracter);

            if(!isFinal)
                return str.Substring (0, position).Trim ();
            else
                return str.Substring ((position), (str.Length-position)).Trim ();
        }

        public static string RemoveCaracter (this String str, string caracter) {

                return str.Replace (caracter, string.Empty).Trim ();
        }

    }
}