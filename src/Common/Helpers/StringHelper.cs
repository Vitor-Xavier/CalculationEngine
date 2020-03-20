using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public static class StringHelper
    {
        public static Regex TrimRegex = new Regex(@"\s+", RegexOptions.Multiline | RegexOptions.Compiled);

        public static string NormalizeWhiteSpace(string text) =>
            TrimRegex.Replace(text, string.Empty).Trim();
    }
}
