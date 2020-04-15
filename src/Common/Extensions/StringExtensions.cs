using System.ComponentModel;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Retorna o texto até a posição do caractere informado.
        /// </summary>
        /// <param name="str">Texto para busca</param>
        /// <param name="caracter">Caractere para busca</param>
        /// <param name="isFinal">Determina se a busca deve partir no final do texto</param>
        /// <returns></returns>
        public static string SubstringWithIndexOf(this string str, char caracter, bool isFinal = false)
        {
            int position = str.IndexOf(caracter);

            if (position < 0)
                return str;

            return isFinal ? str[^(str.Length - position)..].Trim() : str[..position].Trim();
        }

        /// <summary>
        /// Retorna texto sem o caractere informado.
        /// </summary>
        /// <param name="str">Texto para manipulação</param>
        /// <param name="caracter">Caractere para remoção</param>
        /// <returns>Texto sem o caractere informado</returns>
        public static string RemoveCaracter(this string str, string caracter) =>
            str.Replace(caracter, string.Empty).Trim();

        /// <summary>
        /// Converte um texto em anulável do tipo informado.
        /// </summary>
        /// <typeparam name="T">Tipo para conversão</typeparam>
        /// <param name="s">Texto para conversão</param>
        /// <returns>Texto convertido em tipo anulável</returns>
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