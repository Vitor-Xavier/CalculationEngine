using Antlr4.Runtime;
using Crosscutting.DTO.DynamicSearch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers
{
    public static class LanguageHelper
    {
        private static readonly string GlobalConstant = "Roteiro";

        private static readonly string ExternalParam = "Variavel";

        public static IEnumerable<Table> GetTables(IToken[] tokens)
        {
            // Busca por constantes globais, exemplo: '@NomeTabela'
            var tokensVarTable = tokens.Where(x => LanguageLexer.VAR_PRIMARY == x.Type).Select(x => new Table
            {
                Name = x.Text[1..],
                Columns = Array.Empty<Column>()
            });

            // Busca por constantes globais acessando propriedade de objetos, exemplo: '@NomeTabela.NomeColuna'
            var tokensVarTableObject = tokens.Where(x => LanguageLexer.VAR_PRIMARY == x.Type && tokens[x.TokenIndex + 1]?.Type == LanguageLexer.DOT &&
              tokens[x.TokenIndex + 2]?.Type == LanguageLexer.IDENTIFIER).Select(x => new Table
              {
                  Name = x.Text[1..],
                  Columns = new HashSet<Column> { new Column { Name = tokens[x.TokenIndex + 2].Text } }
              });

            // Busca por constantes globais acessando propriedade de lista, exemplo: '@NomeTabela[0].NomeColuna'
            var tokensVarTableList = tokens.Where(x => LanguageLexer.VAR_PRIMARY == x.Type && tokens[x.TokenIndex + 1]?.Type == LanguageLexer.LBRACKET &&
              (tokens[x.TokenIndex + 2]?.Type == LanguageLexer.IDENTIFIER || tokens[x.TokenIndex + 2]?.Type == LanguageLexer.NUMBER) && tokens[x.TokenIndex + 3]?.Type == LanguageLexer.RBRACKET &&
              tokens[x.TokenIndex + 4]?.Type == LanguageLexer.DOT && tokens[x.TokenIndex + 5]?.Type == LanguageLexer.IDENTIFIER).Select(x => new Table
              {
                  Name = x.Text[1..],
                  Columns = new HashSet<Column> { new Column { Name = tokens[x.TokenIndex + 5].Text } }
              });

            return tokensVarTable.Union(tokensVarTableObject).Union(tokensVarTableList)
                .Where(x => x.Name != GlobalConstant && x.Name != ExternalParam)
              .GroupBy(x => x.Name).Select(x => new Table
              {
                  Name = x.Key,
                  Columns = x.SelectMany(c => c.Columns).Select(c => c.Name).Distinct().Select(c => new Column { Name = c }).ToList()
              });
        }

        public static IEnumerable<string> GetDependencies(IToken[] tokens)
        {
            // Busca por fórmulas relacionadas acessando propriedade de objetos, exemplo: '@Roteiro.NomeFormula'
            var tokensVarTableObject = tokens.Where(x => LanguageLexer.IDENTIFIER == x.Type && tokens[x.TokenIndex + 1]?.Type == LanguageLexer.DOT &&
              tokens[x.TokenIndex + 2]?.Type == LanguageLexer.IDENTIFIER).Select(x => tokens[x.TokenIndex + 2].Text);

            // Busca por listas de fórmulas relacionadas acessando propriedade de lista, exemplo: '@Roteiro.NomeFormula[0]'
            var tokensVarTableList = tokens.Where(x => LanguageLexer.IDENTIFIER == x.Type && tokens[x.TokenIndex + 1]?.Type == LanguageLexer.DOT &&
              tokens[x.TokenIndex + 2]?.Type == LanguageLexer.IDENTIFIER && tokens[x.TokenIndex + 3]?.Type == LanguageLexer.LBRACKET &&
              (tokens[x.TokenIndex + 4]?.Type == LanguageLexer.IDENTIFIER || tokens[x.TokenIndex + 4]?.Type == LanguageLexer.NUMBER) && tokens[x.TokenIndex + 5]?.Type == LanguageLexer.RBRACKET)
              .Select(x => tokens[x.TokenIndex + 2].Text);

            return tokensVarTableObject.Union(tokensVarTableList);
        }

        public static IEnumerable<string> GetExternal(IToken[] tokens)
        {
            // Busca por fórmulas relacionadas acessando propriedade de objetos, exemplo: '@Varialvel.NomeValor'
            var tokensVarTableObject = tokens.Where(x => LanguageLexer.IDENTIFIER == x.Type && tokens[x.TokenIndex + 1]?.Type == LanguageLexer.DOT &&
              tokens[x.TokenIndex + 2]?.Type == LanguageLexer.IDENTIFIER).Select(x => tokens[x.TokenIndex + 2].Text);

            return tokensVarTableObject;
        }
    }
}
