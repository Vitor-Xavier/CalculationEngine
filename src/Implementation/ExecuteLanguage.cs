using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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

    public GenericValueLanguage Execute(IDictionary<string, GenericValueLanguage> values = null)
    {
        try
        {
            var visitor = new VisitorLanguage(values);
            return visitor.Visit(_defaultParserTree);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}