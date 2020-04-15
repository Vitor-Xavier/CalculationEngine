using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Substitui espaços em branco em sequência por espaços únicos.
        /// </summary>
        public static Regex TrimRegex = new Regex(@"\s+", RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Retorna texto com a substituição de espaços em branco em sequência por espaços únicos, e sem espaços em branco no início e término do texto.
        /// </summary>
        /// <param name="text">Texto para manupulação</param>
        /// <returns>Texto normalizado</returns>
        public static string NormalizeWhiteSpace(string text) =>
            TrimRegex.Replace(text, string.Empty).Trim();
    }
}
