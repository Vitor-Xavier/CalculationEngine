using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Linq;

namespace Core.Implementation
{
    public class ExecuteLanguage
    {
        public LanguageErrorListener LanguageErrorListener { get; } = new LanguageErrorListener();

        public IParseTree GetParserTree(string input)
        {
            var lexer = new LanguageLexer(new AntlrInputStream(input));
            var commonToken = new CommonTokenStream(lexer);
            var parser = new LanguageParser(commonToken);

#if RELEASE
            parser.Interpreter.PredictionMode = Antlr4.Runtime.Atn.PredictionMode.SLL;
#else
            parser.Interpreter.PredictionMode = Antlr4.Runtime.Atn.PredictionMode.LL;
#endif

            parser.RemoveErrorListeners();
            parser.AddErrorListener(LanguageErrorListener);

            return parser.rule_set();
        }

        public IToken[] GetTokens(string input)
        {
            var lexer = new LanguageLexer(new AntlrInputStream(input));
            var commonToken = new CommonTokenStream(lexer);
            var parser = new LanguageParser(commonToken);

            parser.rule_set();

            return commonToken.GetTokens().ToArray();
        }

        public GenericValueLanguage Execute(string input, IDictionary<string, GenericValueLanguage> values)
        {
            var lexer = new LanguageLexer(new AntlrInputStream(input));
            var commonToken = new CommonTokenStream(lexer);
            var parser = new LanguageParser(commonToken);

            IParseTree parseTree = parser.rule_set();

            var visitor = new LanguageVisitor(values);
            return visitor.Visit(parseTree);
        }

        public GenericValueLanguage ExecuteParseTree(IParseTree parseTree, IDictionary<string, GenericValueLanguage> values)
        {
            var visitor = new LanguageVisitor(values);
            return visitor.Visit(parseTree);
        }
    }
}
