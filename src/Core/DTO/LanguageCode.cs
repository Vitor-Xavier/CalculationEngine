using Antlr4.Runtime.Tree;

namespace Core.DTO
{
    public class LanguageCode
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public IParseTree ParseTree { get; set; }
    }
}
