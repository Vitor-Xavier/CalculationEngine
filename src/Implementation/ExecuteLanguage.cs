using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

public class ExecuteLanguage
{
        public List<object> eita = new List<object>();

        private readonly IParseTree _defaultParserTree;

        public ExecuteLanguage(string executionCode)
        {
            _defaultParserTree = DefaultParserTree(executionCode);
        }

        private static IParseTree DefaultParserTree(string executionCode)
        {
            var lexer = new LanguageLexer(new AntlrInputStream(executionCode));
            var parser = new LanguageParser(new CommonTokenStream(lexer));
            return parser.rule_set();
        }

        public object Execute(Dictionary<string, GenericValueLanguage> values = null)
        {
            var visitor = new VisitorLanguage();

            if (visitor != null)
            foreach (var item in values)
                visitor.memory.Add(item.Key, item.Value);

            try
            {
                var teste = visitor.Visit(_defaultParserTree);
                return teste; 
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
