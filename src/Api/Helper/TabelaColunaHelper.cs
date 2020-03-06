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

    public static List<CaracteristicaTabela> ListaCaracteristicaTabela(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
    {

      // Obter Token das respectivas Key
      var valueTokenCaracteristicaTabela = TokenTypeMap.Where(x => x.Key == "CARACTERISTICA_TABELA").Select(x => x.Value).FirstOrDefault();
      var valueTokenRParen = TokenTypeMap.Where(x => x.Key == "RPAREN").Select(x => x.Value).FirstOrDefault();
      var valueTokenText = TokenTypeMap.Where(x => x.Key == "TEXT").Select(x => x.Value).FirstOrDefault();
      var valueTokenNumber = TokenTypeMap.Where(x => x.Key == "NUMBER").Select(x => x.Value).FirstOrDefault();


      bool tokensEOF = true;
      int tokenIndex = 0;

      List<CaracteristicaTabela> caracteristicaTabelaList = new List<CaracteristicaTabela>();
      while (tokensEOF)
      {

        var getTokenCaracteristicaTabela = Tokens.Where(x => x.Type == valueTokenCaracteristicaTabela && x.TokenIndex > tokenIndex).FirstOrDefault();
        var getTokenRParen = Tokens.Where(x => x.Type == valueTokenRParen && x.TokenIndex > getTokenCaracteristicaTabela?.TokenIndex).FirstOrDefault();

        if (getTokenCaracteristicaTabela is null || getTokenRParen is null)
        {
          tokensEOF = false;
          continue;
        }

        int indexTokenCaracteristicaTabela = getTokenCaracteristicaTabela.TokenIndex;
        int indexTokenRParen = getTokenRParen.TokenIndex;

        var rangeTokenCaracteristicaTabela = Tokens.ToArray()[indexTokenCaracteristicaTabela..indexTokenRParen];

        CaracteristicaTabela caracteristicaValores = new CaracteristicaTabela()
        {
          Tabela = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela, 0).Replace("\"",""),
          Descricao = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela, 1).Replace("\"",""),
          Coluna = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela, 2).Replace("\"",""),
          ValorFator = AntlrHelper.ExtractTextToken(valueTokenText, rangeTokenCaracteristicaTabela, 3).Replace("\"",""),
          Exercicio = AntlrHelper.ExtractTextToken(valueTokenNumber, rangeTokenCaracteristicaTabela, 0),
        };
        caracteristicaTabelaList.Add(caracteristicaValores);
        tokenIndex = getTokenRParen.TokenIndex;
      }
      return caracteristicaTabelaList;
    }

  public static List<Caracteristica> ListaCaracteristica(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
    {

      // Obter Token das respectivas Key
      var valueTokenBuscarCaracteristica = TokenTypeMap.Where(x => x.Key == "CARACTERISTICA").Select(x => x.Value).FirstOrDefault();
      var valueTokenRParen = TokenTypeMap.Where(x => x.Key == "RPAREN").Select(x => x.Value).FirstOrDefault();
      var valueTokenTableCaracteristica = TokenTypeMap.Where(x => x.Key == "TEXT").Select(x => x.Value).FirstOrDefault();
      var valueTokenNumber = TokenTypeMap.Where(x => x.Key == "NUMBER").Select(x => x.Value).FirstOrDefault();


      bool tokensEOF = true;
      int tokenIndex = 0;

      List<Caracteristica> caracteristicaParametrosList = new List<Caracteristica>();
      while (tokensEOF)
      {

        var getTokenBuscarCaracteristica = Tokens.Where(x => x.Type == valueTokenBuscarCaracteristica && x.TokenIndex > tokenIndex).FirstOrDefault();
        var getTokenRParen = Tokens.Where(x => x.Type == valueTokenRParen && x.TokenIndex > getTokenBuscarCaracteristica?.TokenIndex).FirstOrDefault();

        if (getTokenBuscarCaracteristica is null || getTokenRParen is null)
        {
          tokensEOF = false;
          continue;
        }

        int indexTokenBuscarCaracteristica = getTokenBuscarCaracteristica.TokenIndex;
        int indexTokenRParen = getTokenRParen.TokenIndex;

        var rangeTokenBuscaCaracteristica = Tokens.ToArray()[indexTokenBuscarCaracteristica..indexTokenRParen];

        Caracteristica caracteristicaValores = new Caracteristica()
        {
          Descricao = AntlrHelper.ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 0).Replace("\"",""),
          Codigo = AntlrHelper.ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 1).Replace("\"",""),
          ValorFator = AntlrHelper.ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 2).Replace("\"",""),
          Exercicio = AntlrHelper.ExtractTextToken(valueTokenNumber, rangeTokenBuscaCaracteristica, 0),
        };
        caracteristicaParametrosList.Add(caracteristicaValores);
        tokenIndex = getTokenRParen.TokenIndex;
      }
      return caracteristicaParametrosList;
    }
  }
}