using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;

public class ExecuteLanguage
{
    private IParseTree _defaultParserTree;
    public CommonTokenStream commonToken;
    public LanguageParser parser;
    private readonly IDictionary<string, GenericValueLanguage> _memoryGlobal;

    public ExecuteLanguage(IDictionary<string, GenericValueLanguage> memoryGlobal)
    {
        _memoryGlobal = memoryGlobal;
    }

    public ExecuteLanguage()
    {
        
    }
    public IParseTree DefaultParserTree(string executionCode)
    {
        var lexer = new LanguageLexer(new AntlrInputStream(executionCode));
        commonToken = new CommonTokenStream(lexer);
        parser = new LanguageParser(commonToken);

        _defaultParserTree = parser.rule_set();
        return parser.rule_set();
    }

    public GenericValueLanguage Execute(IDictionary<string, GenericValueLanguage> values = null)
    {
        var visitor = new VisitorLanguage(values, _memoryGlobal);
        return visitor.Visit(_defaultParserTree);
    }
}