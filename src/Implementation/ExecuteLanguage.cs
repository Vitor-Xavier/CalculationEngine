using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;

public class ExecuteLanguage
{
    private IParseTree _defaultParserTree;
    public CommonTokenStream commonToken;
    public LanguageParser parser;

    public IParseTree DefaultParserTree(string executionCode)
    {
        var lexer = new LanguageLexer(new AntlrInputStream(executionCode));
        commonToken = new CommonTokenStream(lexer);
        parser = new LanguageParser(commonToken);

        _defaultParserTree = parser.rule_set();
        return parser.rule_set();
    }

    public GenericValueLanguage Execute(IDictionary<string, GenericValueLanguage> values, IDictionary<string, IEnumerable<object>> global)
    {
        var visitor = new VisitorLanguage(values, global);
        return visitor.Visit(_defaultParserTree);
    }
}