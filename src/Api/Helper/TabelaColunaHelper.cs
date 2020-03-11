using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Api.Dto;

namespace Api.Helper
{
    public static class TabelaColunaHelper
    {
        public static IEnumerable<TabelaColuna> GetTabelaColunas(IEnumerable<Evento> eventos)
        {
            var grupo = new List<TabelaColuna>();

            foreach (var evento in eventos)
            {
                ExecuteLanguage execute = new ExecuteLanguage();
                execute.DefaultParserTree(evento.Formula);

                var tokens = execute.commonToken.GetTokens();
                var tokenTypeMap = execute.parser.TokenTypeMap;

                // Separa Massa de Dados para Buscar no Banco de Dados
                grupo.AddRange(ListaTabelaColuna(tokens, tokenTypeMap));

            }
            grupo = grupo.GroupBy(t => t.Tabela).Select(x => new TabelaColuna { Tabela = x.Key, Coluna = x.SelectMany(y => y.Coluna).Distinct().ToList() }).ToList();
            return grupo;
        }

        public static IEnumerable<CaracteristicaTabela> GetCaracteristicaTabela(IEnumerable<Evento> eventos)
        {

            var caracteristicaTabela = new List<CaracteristicaTabela>();

            foreach (var evento in eventos)
            {
                ExecuteLanguage execute = new ExecuteLanguage();
                execute.DefaultParserTree(evento.Formula);

                var tokens = execute.commonToken.GetTokens();
                var tokenTypeMap = execute.parser.TokenTypeMap;

                // Separa Massa de Dados para Buscar no Banco de Dados
                caracteristicaTabela.AddRange(ListaCaracteristicaTabela(tokens, tokenTypeMap));

            }

            caracteristicaTabela = caracteristicaTabela
                .GroupBy(x => new
                {
                    TabelaCaracteristica = x.Tabela,
                    DescricaoCaracteristica = x.Descricao,
                    ColunaCaracteristica = x.Coluna,
                    ExercicioCaracteristica = x.Exercicio,
                    ValorFatorCaracteristica = x.ValorFator
                })
                .Select(x => new CaracteristicaTabela
                {
                    Tabela = x.Key.TabelaCaracteristica,
                    Descricao = x.Key.DescricaoCaracteristica,
                    Coluna = x.Key.ColunaCaracteristica,
                    Exercicio = x.Key.ExercicioCaracteristica,
                    ValorFator = x.Key.ValorFatorCaracteristica,
                }).ToList();

            return caracteristicaTabela;
        }

        public static IEnumerable<Caracteristica> GetCaracteristica(IEnumerable<Evento> eventos)
        {

            var caracteristica = new List<Caracteristica>();

            foreach (var evento in eventos)
            {
                ExecuteLanguage execute = new ExecuteLanguage();
                execute.DefaultParserTree(evento.Formula);

                var tokens = execute.commonToken.GetTokens();
                var tokenTypeMap = execute.parser.TokenTypeMap;

                // Separa Massa de Dados para Buscar no Banco de Dados
                caracteristica.AddRange(ListaCaracteristica(tokens, tokenTypeMap));

            }

            caracteristica = caracteristica
                .GroupBy(x => new
                {
                    Descricao = x.Descricao,
                    Codigo = x.Codigo,
                    Exercicio = x.Exercicio,
                    ValorFator = x.ValorFator
                })
                .Select(x => new Caracteristica
                {
                    Descricao = x.Key.Descricao,
                    Codigo = x.Key.Codigo,
                    Exercicio = x.Key.Exercicio,
                    ValorFator = x.Key.ValorFator,
                }).ToList();

            return caracteristica;
        }

        public static IEnumerable<TabelaColuna> ListaTabelaColuna(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
        {
            var tokenVarTableColuna = TokenTypeMap.Where(x => x.Key == "VAR_OBJECT" || x.Key == "VAR_ARRAY").Select(x => x.Value).ToList();
            var tokensTypeVarTableColuna = Tokens.Where(x => tokenVarTableColuna.Contains(x.Type)).ToList();
            var grupo = tokensTypeVarTableColuna.Select(x => new
            {
                tabela = Regex.Replace(x.Text.RemoveCaracter("@").SubstringWithIndexOf('.'), @"\[\w+\]", string.Empty),
                coluna = x.Text.RemoveCaracter("@").SubstringWithIndexOf('.', true).RemoveCaracter(".")
            })
                .Where(u => u.tabela != "Roteiro")
                .GroupBy(u => u.tabela)
                .Select(y => new TabelaColuna
                {
                    Tabela = y.Select(y => y.tabela).Distinct().FirstOrDefault(),
                    Coluna = y.Select(y => y.coluna).Distinct().ToList()
                })
                .ToList();
            return grupo;
        }

        public static IEnumerable<CaracteristicaTabela> ListaCaracteristicaTabela(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
        {
            // Obter Token das respectivas Key
            int valueTokenCaracteristicaTabela = TokenTypeMap.FirstOrDefault(x => x.Key == "CARACTERISTICA_TABELA").Value;
            int valueTokenRParen = TokenTypeMap.FirstOrDefault(x => x.Key == "RPAREN").Value;
            int valueTokenText = TokenTypeMap.FirstOrDefault(x => x.Key == "TEXT").Value;
            int valueTokenNumber = TokenTypeMap.FirstOrDefault(x => x.Key == "NUMBER").Value;

            int tokenIndex = 0;
            while (Tokens.FirstOrDefault(x => x.Type == valueTokenCaracteristicaTabela && x.TokenIndex > tokenIndex) is IToken tokenCaracteristicaTabela)
            {
                var tokenRParen = Tokens.FirstOrDefault(x => x.Type == valueTokenRParen && x.TokenIndex > tokenCaracteristicaTabela.TokenIndex);

                var rangeTokenCaracteristicaTabela = Tokens.ToArray()[tokenCaracteristicaTabela.TokenIndex..tokenRParen.TokenIndex];

                yield return new CaracteristicaTabela
                {
                    Tabela = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela, 0).Replace("\"", ""),
                    Descricao = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela, 1).Replace("\"", ""),
                    Coluna = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela, 2).Replace("\"", ""),
                    ValorFator = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela, 3).Replace("\"", ""),
                    Exercicio = AntlrHelper.ExtractTextToken(valueTokenNumber, rangeTokenCaracteristicaTabela, 0),
                };

                tokenIndex = tokenRParen.TokenIndex;
            }
        }

        public static IEnumerable<Caracteristica> ListaCaracteristica(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
        {
            // Obter Token das respectivas Key
            int valueTokenBuscarCaracteristica = TokenTypeMap.FirstOrDefault(x => x.Key == "CARACTERISTICA").Value;
            int valueTokenRParen = TokenTypeMap.FirstOrDefault(x => x.Key == "RPAREN").Value;
            int valueTokenTableCaracteristica = TokenTypeMap.FirstOrDefault(x => x.Key == "TEXT").Value;
            int valueTokenNumber = TokenTypeMap.FirstOrDefault(x => x.Key == "NUMBER").Value;

            int tokenIndex = 0;
            while (Tokens.FirstOrDefault(x => x.Type == valueTokenBuscarCaracteristica && x.TokenIndex > tokenIndex) is IToken tokenBuscarCaracteristica)
            {
                var tokenRParen = Tokens.FirstOrDefault(x => x.Type == valueTokenRParen && x.TokenIndex > tokenBuscarCaracteristica.TokenIndex);

                var rangeTokenBuscaCaracteristica = Tokens.ToArray()[tokenBuscarCaracteristica.TokenIndex..tokenRParen.TokenIndex];

                yield return new Caracteristica
                {
                    Descricao = AntlrHelper.ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 0).Replace("\"", ""),
                    Codigo = AntlrHelper.ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 1).Replace("\"", ""),
                    ValorFator = AntlrHelper.ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 2).Replace("\"", ""),
                    Exercicio = AntlrHelper.ExtractTextToken(valueTokenNumber, rangeTokenBuscaCaracteristica, 0),
                };
                tokenIndex = tokenRParen.TokenIndex;
            }
        }

        public static IEnumerable<Parametro> GetParametros(IEnumerable<Evento> eventos)
        {
            var parametros = new List<Parametro>();

            foreach (var evento in eventos)
            {
                ExecuteLanguage execute = new ExecuteLanguage();
                execute.DefaultParserTree(evento.Formula);

                var tokens = execute.commonToken.GetTokens();
                var tokenTypeMap = execute.parser.TokenTypeMap;

                // Separa Massa de Dados para Buscar no Banco de Dados
                parametros.AddRange(GetParametros(tokens, tokenTypeMap));
            }
            return parametros;
        }

        public static IEnumerable<Parametro> GetParametros(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
        {
            int tokenValueParametro = TokenTypeMap.FirstOrDefault(x => x.Key == "PARAMETRO").Value;
            int tokenValueParametroCodigo = TokenTypeMap.FirstOrDefault(x => x.Key == "PARAMETRO_CODIGO").Value;
            int tokenValueParametroIntervalo = TokenTypeMap.FirstOrDefault(x => x.Key == "PARAMETRO_INTERVALO").Value;
            int tokenValueLParen = TokenTypeMap.FirstOrDefault(x => x.Key == "LPAREN").Value;
            int tokenValueRParen = TokenTypeMap.FirstOrDefault(x => x.Key == "RPAREN").Value;
            int tokenValueText = TokenTypeMap.FirstOrDefault(x => x.Key == "TEXT").Value;
            int tokenValueNumber = TokenTypeMap.FirstOrDefault(x => x.Key == "NUMBER").Value;

            int tokenIndex = 0;
            while (Tokens.FirstOrDefault(x => (x.Type == tokenValueParametro ||
                x.Type == tokenValueParametroCodigo ||
                x.Type == tokenValueParametroIntervalo) &&
                x.TokenIndex > tokenIndex) is IToken tokenParametro)
            {
                var tokenRParen = Tokens.FirstOrDefault(x => x.Type == tokenValueRParen && x.TokenIndex > tokenParametro.TokenIndex);

                var rangeToken = Tokens.ToArray()[tokenParametro.TokenIndex..tokenRParen.TokenIndex];

                yield return new Parametro
                {
                    Nome = AntlrHelper.ExtractTextToken(tokenValueText, rangeToken, 0).Replace("\"", ""),
                    Codigo = tokenParametro.Type == tokenValueParametroCodigo ? AntlrHelper.ExtractTextToken(tokenValueText, rangeToken, 1).Replace("\"", "") : null,
                    Valor = tokenParametro.Type == tokenValueParametroIntervalo ? AntlrHelper.ExtractTextToken(tokenValueText, rangeToken, 1).Replace("\"", "") : null,
                    Exercicio = int.TryParse(AntlrHelper.ExtractTextToken(tokenValueNumber, rangeToken, 0), out int exercicio) ? exercicio : default(int?)
                };

                tokenIndex = tokenRParen.TokenIndex;
            }
        }
    }
}