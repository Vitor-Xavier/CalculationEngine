using Antlr4.Runtime;
using Api.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Api.Helper
{
    public static class TabelaColunaHelper
    {
        public static IEnumerable<TabelaColuna> GetTabelaColunas(IEnumerable<Evento> eventos)
        {
            var grupo = new List<TabelaColuna>();
            var caracteristicaParametros = new List<CaracteristicaParametros>();

            foreach (var evento in eventos)
            {
                ExecuteLanguage execute = new ExecuteLanguage();
                execute.DefaultParserTree(evento.Formula);

                var tokens = execute.commonToken.GetTokens();
                var tokenTypeMap = execute.parser.TokenTypeMap;

                // Separa Massa de Dados para Buscar no Banco de Dados
                caracteristicaParametros.AddRange(ListaCaracteristicaParametros(tokens, tokenTypeMap));
                grupo.AddRange(ListaTabelaColuna(tokens, tokenTypeMap));

            }
            grupo = grupo.GroupBy(t => t.Tabela).Select(x => new TabelaColuna { Tabela = x.Key, Coluna = x.SelectMany(y => y.Coluna).Distinct().ToList() }).ToList();
            return grupo;
        }

        public static IEnumerable<TabelaColuna> ListaTabelaColuna(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
        {
            List<TabelaColuna> grupo;
            var tokenVarTableColuna = TokenTypeMap.Where(x => x.Key == "VAR_TABLE_COLUNA").FirstOrDefault();
            var tokensTypeVarTableColuna = Tokens.Where(x => x.Type == tokenVarTableColuna.Value).ToList();
            grupo = tokensTypeVarTableColuna.Select(x => new
            {
                tabela = Regex.Replace(x.Text.RemoveCaracter("@").SubstringWithIndexOf('.'), @"\[[0-9]+\]", string.Empty),
                coluna = x.Text.RemoveCaracter("@").SubstringWithIndexOf('.', true).RemoveCaracter(".")
            })
                .Where(u => u.tabela != "Roteiro")
                .GroupBy(u => u.tabela)
                .Select(y => new TabelaColuna()
                {
                    Tabela = y.Select(y => y.tabela).Distinct().FirstOrDefault(),
                    Coluna = y.Select(y => y.coluna).ToList()
                })
                .ToList();
            return grupo;
        }

        public static List<CaracteristicaParametros> ListaCaracteristicaParametros(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
        {
            var tokenBuscarCaracteristica = TokenTypeMap.Where(x => x.Key == "BUSCAR_CARACTERISTICA").FirstOrDefault();
            var tokenRParen = TokenTypeMap.Where(x => x.Key == "RPAREN").FirstOrDefault();
            var valueTokenTableCaracteristica = TokenTypeMap.Where(x => x.Key == "IDENTIFIER").Select(x => x.Value).FirstOrDefault();
            var valueTokenNumber = TokenTypeMap.Where(x => x.Key == "NUMBER").Select(x => x.Value).FirstOrDefault();
            bool tokensEOF = true;
            int tokenIndex = 0;
            List<CaracteristicaParametros> caracteristicaParametrosList = new List<CaracteristicaParametros>();
            while (tokensEOF)
            {
                var getTokenBuscarCaracteristica = Tokens.Where(x => x.Type == tokenBuscarCaracteristica.Value && x.TokenIndex > tokenIndex).FirstOrDefault();
                var getTokenRParen = Tokens.Where(x => x.Type == tokenRParen.Value && x.TokenIndex > tokenIndex).FirstOrDefault();
                if (getTokenBuscarCaracteristica is null || getTokenRParen is null)
                {
                    tokensEOF = false;
                    continue;
                }
                int indexTokenBuscarCaracteristica = getTokenBuscarCaracteristica.TokenIndex;
                int indexTokenRParen = getTokenRParen.TokenIndex;
                var rangeTokenBuscaCaracteristica = Tokens.ToArray()[indexTokenBuscarCaracteristica..indexTokenRParen];
                CaracteristicaParametros caracteristicaValores = new CaracteristicaParametros()
                {
                    TabelaCaracteristica = AntlrHelper.ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 0),
                    DescricaoCaracteristica = AntlrHelper.ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 1),
                    ColunaCaracteristica = AntlrHelper.ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 2),
                    ColunaValorCaracteristica = AntlrHelper.ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 3),
                    ExercicioCaracteristica = AntlrHelper.ExtractTextToken(valueTokenNumber, rangeTokenBuscaCaracteristica, 0),
                };
                caracteristicaParametrosList.Add(caracteristicaValores);
                tokenIndex = getTokenRParen.TokenIndex;
            }
            return caracteristicaParametrosList;
        }
    }
}
