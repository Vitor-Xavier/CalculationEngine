using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public static class RegexHelper
    {
        public static Regex MissingChar { get; } = new Regex(@"Missing\s((?<char>'(.*?)')|(?<rule>[a-zA-Z0-9._@\[\]]+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Regex ExtraneousInput { get; } = new Regex(@"Extraneous\sinput\s('(?<char>(.*?))'|(?<rule>[a-zA-Z0-9._@\[\]]+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
