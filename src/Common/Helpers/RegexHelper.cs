using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public static class RegexHelper
    {
        /// <summary>
        /// Busca por correspondências de erro por caractere ou identificador ausente.
        /// </summary>
        public static Regex MissingChar { get; } = new Regex(@"Missing\s((?<char>'(.*?)')|(?<rule>[a-zA-Z0-9._@\[\]]+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Busca por correspondências de erro por entrada inesperada de identificador ou caractere.
        /// </summary>
        public static Regex ExtraneousInput { get; } = new Regex(@"Extraneous\sinput\s('(?<char>(.*?))'|(?<rule>[a-zA-Z0-9._@\[\]]+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
