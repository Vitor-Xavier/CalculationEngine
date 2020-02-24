using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

public class ExecuteLanguage {
    public List<object> eita = new List<object> ();

    private IParseTree _defaultParserTree;

    public ExecuteLanguage () {
        // _defaultParserTree = DefaultParserTree(executionCode);
    }

    public IParseTree DefaultParserTree (string executionCode) {
        var lexer = new LanguageLexer (new AntlrInputStream (executionCode));
        var commonToken = new CommonTokenStream (lexer);
        var parser = new LanguageParser (commonToken);

        _defaultParserTree = parser.rule_set ();
        return parser.rule_set ();
    }

    public object Execute (Dictionary<string, GenericValueLanguage> values = null) {
        var visitor = new VisitorLanguage ();

        if (visitor != null)

            if (values != null) {
                foreach (var item in values)
                    visitor.memory.Add (item.Key, item.Value);
            }

        try {
            var teste = visitor.Visit (_defaultParserTree);

            return teste;
        } catch (Exception e) {
            throw e;
        }
    }
}