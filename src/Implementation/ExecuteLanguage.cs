using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;

namespace Implementation
{
    public class ExecuteLanguage
    {
        public CommonTokenStream commonToken;
        public LanguageParser parser;
        private readonly IDictionary<string, GenericValueLanguage> _memoryGlobal;

        public LanguageErrorListener LanguageErrorListener { get; } = new LanguageErrorListener();

        public ExecuteLanguage() => _memoryGlobal = new Dictionary<string, GenericValueLanguage>();

        public ExecuteLanguage(IDictionary<string, GenericValueLanguage> memoryGlobal) =>
            _memoryGlobal = memoryGlobal;

        public IParseTree DefaultParserTree(string executionCode)
        {
            var lexer = new LanguageLexer(new AntlrInputStream(executionCode));
            commonToken = new CommonTokenStream(lexer);
            parser = new LanguageParser(commonToken);

#if RELEASE
        parser.Interpreter.PredictionMode = Antlr4.Runtime.Atn.PredictionMode.SLL;
#else
            parser.Interpreter.PredictionMode = Antlr4.Runtime.Atn.PredictionMode.LL;
#endif

            parser.RemoveErrorListeners();
            parser.AddErrorListener(LanguageErrorListener);

            return parser.rule_set();
        }

        public GenericValueLanguage Execute(IDictionary<string, GenericValueLanguage> values, IParseTree parseTree)
        {
            var visitor = new VisitorLanguage(values, _memoryGlobal);
            return visitor.Visit(parseTree);
        }
    }
}
