using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Api.Dto;

namespace Api.Helper
{
  public static class TabelaColunaHelper
  {
    public static IEnumerable<TabelaColuna> GetTabelaColunas(Roteiro roteiro)
    {

      var grupo = new List<TabelaColuna>();

      string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(roteiro.SetorOrigem);
      string colunaPrincipal = SetorOrigemHelper.GetTabelaPrincipalColuna(roteiro.SetorOrigem);
      foreach (var evento in roteiro.Eventos)
      {
        ExecuteLanguage execute = new ExecuteLanguage();
        execute.DefaultParserTree(evento.Formula);

        var tokens = execute.commonToken.GetTokens();
        var tokenTypeMap = execute.parser.TokenTypeMap;

        // Separa Massa de Dados para Buscar no Banco de Dados
        grupo.AddRange(ListaTabelaColuna(tokens, tokenTypeMap));

      }

      if (!grupo.Any(x => x.Tabela == tabelaPrincipal))
      {
        grupo.Add(new TabelaColuna()
        {
          Tabela = tabelaPrincipal,
          Coluna = new List<string>() { colunaPrincipal }
        });
      }
      else
      {
        grupo.Where(x => x.Tabela == tabelaPrincipal).Select(c => { c.Coluna.Add(colunaPrincipal); return c; }).ToList();
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

    public static List<AtividadeTabela> GetAtividadeTabela(IEnumerable<Evento> eventos)
    {

      List<AtividadeTabela> caracteristicaTabela = new List<AtividadeTabela>();

      foreach (var evento in eventos)
      {
        ExecuteLanguage execute = new ExecuteLanguage();
        execute.DefaultParserTree(evento.Formula);

        var tokens = execute.commonToken.GetTokens();
        var tokenTypeMap = execute.parser.TokenTypeMap;

        // Separa Massa de Dados para Buscar no Banco de Dados
        caracteristicaTabela.AddRange(ListaAtividadeTabela(tokens, tokenTypeMap));

      }

      caracteristicaTabela = caracteristicaTabela
      .GroupBy(x => new
      {
        Descricao = x.Descricao,
        Coluna = x.Coluna,
        Exercicio = x.Exercicio,
        Tabela = x.Tabela,
        ColunaFiltro = x.ColunaFiltro,
      })
      .Select(x => new AtividadeTabela
      {
        Descricao = x.Key.Descricao,
        Coluna = x.Key.Coluna,
        Exercicio = x.Key.Exercicio,
        Tabela = x.Key.Tabela,
        ColunaFiltro = x.Key.ColunaFiltro,
        Colunas = x.SelectMany(u => u.Colunas.Distinct()).ToList().Distinct().ToList()
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
      var tokenVarTableColuna = TokenTypeMap.Where(x => x.Key == "VAR_PRIMARY").Select(x => x.Value).ToList();
      var tokenLBRACKET = TokenTypeMap.Where(x => x.Key == "LBRACKET").Select(x => x.Value).FirstOrDefault();
      var tokenRBRACKET = TokenTypeMap.Where(x => x.Key == "RBRACKET").Select(x => x.Value).FirstOrDefault();
      var tokenDOT = TokenTypeMap.Where(x => x.Key == "DOT").Select(x => x.Value).FirstOrDefault();
      var tokenNUMBER = TokenTypeMap.Where(x => x.Key == "NUMBER").Select(x => x.Value).FirstOrDefault();
      var tokenIDENTIFIER = TokenTypeMap.Where(x => x.Key == "IDENTIFIER").Select(x => x.Value).FirstOrDefault();

      var tokensTypeVarTableColuna = Tokens.Where(x => tokenVarTableColuna.Contains(x.Type)).ToList();

      // @NomeTabela.NomeColuna
      var tokensTypeVarTableColuna4 = Tokens.Where(x => tokenVarTableColuna.Contains(x.Type)).Select(x =>
            Tokens.ToList().Where(y => y.TokenIndex == x.TokenIndex + 1 && y.Type == tokenDOT).Any() ?
            x.Text +
             Tokens.ToList().Where(y => y.TokenIndex == x.TokenIndex + 1 && y.Type == tokenDOT).Select(y => y.Text).FirstOrDefault() +
             Tokens.ToList().Where(y => y.TokenIndex == (x.TokenIndex + 2) && y.Type == tokenIDENTIFIER).Select(y => y.Text).FirstOrDefault()
             : string.Empty
            ).ToList();

      // @NomeTabela[0].NomeColuna
      var tokensTypeVarTableColuna5 = Tokens.Where(x => tokenVarTableColuna.Contains(x.Type)).Select(x =>
            Tokens.ToList().Where(y => y.TokenIndex == x.TokenIndex + 1 && y.Type == tokenLBRACKET).Any() ?
            x.Text +
             Tokens.ToList().Where(y => y.TokenIndex == x.TokenIndex + 1 && y.Type == tokenLBRACKET).Select(y => y.Text).FirstOrDefault() +
             Tokens.ToList().Where(y => y.TokenIndex == x.TokenIndex + 2 && (y.Type == tokenIDENTIFIER || y.Type == tokenNUMBER)).Select(y => y.Text).FirstOrDefault() +
             Tokens.ToList().Where(y => y.TokenIndex == x.TokenIndex + 3 && y.Type == tokenRBRACKET).Select(y => y.Text).FirstOrDefault() +
             Tokens.ToList().Where(y => y.TokenIndex == x.TokenIndex + 4 && y.Type == tokenDOT).Select(y => y.Text).FirstOrDefault() +
             Tokens.ToList().Where(y => y.TokenIndex == x.TokenIndex + 5 && y.Type == tokenIDENTIFIER).Select(y => y.Text).FirstOrDefault()
             : string.Empty
            ).ToList();

      // @Fisico
      var tokensTypeVarTableColuna6 = Tokens.Where(x => tokenVarTableColuna.Contains(x.Type)).Select(x =>
      Tokens.ToList().Where(y => y.TokenIndex == x.TokenIndex + 1 && (y.Type != tokenDOT && y.Type != tokenLBRACKET)).Any() ?
             x.Text : string.Empty
            ).Distinct().Where(x => !string.IsNullOrEmpty(x)).ToList();

      var testeFinal = tokensTypeVarTableColuna6.Union(tokensTypeVarTableColuna5).Union(tokensTypeVarTableColuna4).Where(x => !string.IsNullOrEmpty(x)).ToList();
      var grupo = testeFinal.Select(x => new
      {
        tabela = Regex.Replace(x.ToString().RemoveCaracter("@").SubstringWithIndexOf('.'), @"\[\w+\]", string.Empty),
        coluna = x.ToString().RemoveCaracter("@").SubstringWithIndexOf('.', true).RemoveCaracter(".")
      })
          .Where(u => u.tabela != "Roteiro")
          .GroupBy(u => u.tabela)
          .Select(y => new TabelaColuna
          {
            Tabela = y.Select(y => y.tabela).Distinct().FirstOrDefault(),
            Coluna = y.Where(y => y.coluna != y.tabela).Select(y => y.coluna).Distinct().ToList()
          })
          .ToList();
      return grupo;
    }

    public static List<AtividadeTabela> ListaAtividadeTabela(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
    {
      // Obter Token das respectivas Key
      int valueTokenCaracteristicaTabela = TokenTypeMap.FirstOrDefault(x => x.Key == "ATIVIDADE_TABELA").Value;
      int valueTokenRParen = TokenTypeMap.FirstOrDefault(x => x.Key == "RPAREN").Value;
      int valueTokenLBracket = TokenTypeMap.FirstOrDefault(x => x.Key == "LBRACKET").Value;
      int valueTokenText = TokenTypeMap.FirstOrDefault(x => x.Key == "TEXT").Value;
      int valueTokenNumber = TokenTypeMap.FirstOrDefault(x => x.Key == "NUMBER").Value;

      List<AtividadeTabela> listaAtividade = new List<AtividadeTabela>();
      int tokenIndex = 0;

      while (Tokens.FirstOrDefault(x => x.Type == valueTokenCaracteristicaTabela && x.TokenIndex >= tokenIndex) is IToken tokenCaracteristicaTabela)
      {
        AtividadeTabela teste = new AtividadeTabela();
        var tokenRParen = Tokens.FirstOrDefault(x => x.Type == valueTokenRParen && x.TokenIndex > tokenCaracteristicaTabela.TokenIndex);

        IToken TokenLBracket = Tokens.FirstOrDefault(x => x.Type == valueTokenLBracket && x.TokenIndex > tokenCaracteristicaTabela.TokenIndex && x.TokenIndex >= tokenIndex);

        var rangeTokenCaracteristicaTabela2 = Tokens.ToArray()[tokenCaracteristicaTabela.TokenIndex..TokenLBracket.TokenIndex];
        teste.Tabela = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela2, 0).Replace("\"", "");
        teste.Descricao = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela2, 1).Replace("\"", "");
        teste.Coluna = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela2, 2).Replace("\"", "");
        teste.ColunaFiltro = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela2, 3).Replace("\"", "");
        teste.Exercicio = AntlrHelper.ExtractTextToken(valueTokenNumber, rangeTokenCaracteristicaTabela2, 0);


        var rangeTokenCaracteristicaTabela = Tokens.ToArray()[TokenLBracket.TokenIndex..tokenRParen.TokenIndex];

        var rangeText = rangeTokenCaracteristicaTabela.Where(x => x.Type == valueTokenText).Select(x => x.Text.Replace("\"", "")).ToList();
        teste.Colunas.AddRange(rangeText);


        listaAtividade.Add(teste);
        tokenIndex += tokenRParen.TokenIndex;
      }

      return listaAtividade;
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

      int tokenValueLParen = TokenTypeMap.FirstOrDefault(x => x.Key == "LPAREN").Value;
      int tokenValueRParen = TokenTypeMap.FirstOrDefault(x => x.Key == "RPAREN").Value;
      int tokenValueText = TokenTypeMap.FirstOrDefault(x => x.Key == "TEXT").Value;
      int tokenValueNumber = TokenTypeMap.FirstOrDefault(x => x.Key == "NUMBER").Value;

      int tokenIndex = 0;
      while (Tokens.FirstOrDefault(x => (x.Type == tokenValueParametro ||
          x.Type == tokenValueParametroCodigo) &&
          x.TokenIndex > tokenIndex) is IToken tokenParametro)
      {
        var tokenRParen = Tokens.FirstOrDefault(x => x.Type == tokenValueRParen && x.TokenIndex > tokenParametro.TokenIndex);

        var rangeToken = Tokens.ToArray()[tokenParametro.TokenIndex..tokenRParen.TokenIndex];

        yield return new Parametro
        {
          Nome = AntlrHelper.ExtractTextToken(tokenValueText, rangeToken, 0).Replace("\"", ""),
          Codigo = tokenParametro.Type == tokenValueParametroCodigo ? AntlrHelper.ExtractTextToken(tokenValueText, rangeToken, 1).Replace("\"", "") : null,
          Exercicio = int.TryParse(AntlrHelper.ExtractTextToken(tokenValueNumber, rangeToken, 0), out int exercicio) ? exercicio : 0
        };

        tokenIndex = tokenRParen.TokenIndex;
      }
    }
  }
}