using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

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

        public Object Execute()
        {
           

           
            

            var visitor = new VisitorLanguage();

            

            try
            {
                var teste = visitor.Visit(_defaultParserTree);
                return teste; 
            }
            catch (Exception e)
            {
              

                throw new Exception();

            }

            
        }
    }
