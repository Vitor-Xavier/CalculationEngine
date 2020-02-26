using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using Api.Dto;

namespace Api {
    public class Program {

        static decimal GerarDecimal () {
            Random rnd = new Random ();
            byte scale = (byte) rnd.Next (29);
            bool sign = rnd.Next (2) == 1;
            var teste = new decimal (rnd.Next (10), rnd.Next (10), rnd.Next (10), sign, scale);
            return teste;
        }

        static void Main (string[] args)
    {

      Roteiro roteiro = new Roteiro()
      {
        Id = 1,
        NomeRoteiro = "IPTU",
        SetorOrigem = "Imobiliario",
        Eventos = new List<Evento>()
      };

      Evento fatorG = new Evento()
      {
        Id = 1,
        Nome = "FatorG",
        Formula = @"retorno (@Fisico.AreaTerreno * @Fisico.Testada * @Fisico.AreaTerreno * @FisicoRural.Rural * @FisicoRural.Estado)/30;"
      };

      Evento fatorK = new Evento()
      {
        Id = 2,
        Nome = "FatorK",
        Formula = @"retorno 5725.90;"
      };

      Evento vvt = new Evento()
      {
        Id = 3,
        Nome = "vvt",
        Formula = @"retorno @FatorG * @FatorK * @Fisico.AreaTotalConstruida;"
      };

      Evento vvp = new Evento()
      {
        Id = 4,
        Nome = "vvp",
        Formula = @"retorno @Fisico.AreaEdificada + 200;"
      };

      Evento iptu = new Evento()
      {
        Id = 5,
        Nome = "IPTU",
        Formula = @"retorno @vvp + @vvt;"
      };

      vvt.Eventos = new List<Evento>();
      vvt.Eventos.Add(fatorK);

      iptu.Eventos = new List<Evento>();
      iptu.Eventos.Add(vvp);
      iptu.Eventos.Add(vvt);

      roteiro.Eventos.Add(fatorG);
      roteiro.Eventos.Add(fatorK);
      roteiro.Eventos.Add(vvt);
      roteiro.Eventos.Add(vvp);
      roteiro.Eventos.Add(iptu);

      List<ExemploFisico> listaFisico = new List<ExemploFisico>();

      for (int i = 0; i < 100; i++)
      {
        var novoFisico = new ExemploFisico()
        {
          Id = 0,
          AreaEdificada = GerarDecimal(),
          AreaTerreno = GerarDecimal(),
          Caracteristica = GerarDecimal(),
          CaracteristicaEspecial = GerarDecimal(),
          Conservacao = GerarDecimal(),
          FatorPosicaoQuadra = GerarDecimal(),
          FracaoIdeal = GerarDecimal(),
          LocalPropriedadeLote = GerarDecimal(),
          NumeroFrentes = GerarDecimal(),
          Pedologia = GerarDecimal(),
          Pontos = GerarDecimal(),
          Testada = GerarDecimal(),
          Topografia = GerarDecimal(),
          ValorM = GerarDecimal(),

          Teste01 = GerarDecimal(),
          Teste02 = GerarDecimal(),
          Teste03 = GerarDecimal(),
          Teste04 = GerarDecimal(),
          Teste05 = GerarDecimal(),
          Teste06 = GerarDecimal(),
          Teste07 = GerarDecimal(),

        };
        listaFisico.Add(novoFisico);
      }

      ExecuteLanguage execute = new ExecuteLanguage();

      string F1 = roteiro.Eventos.Where(x => x.Id == 1).Select(x => x.Formula).FirstOrDefault();

      execute.DefaultParserTree(F1);

      List<TabelaColuna> grupo = new List<TabelaColuna>();

      grupo = ListaTabelaColuna(execute);

      var value = execute.Execute();

    }

    private static List<TabelaColuna> ListaTabelaColuna(ExecuteLanguage execute)
    {
      List<TabelaColuna> grupo;
      
      var tokens = execute.commonToken.GetTokens();
      var tokenTypeMap = execute.parser.TokenTypeMap;

      var tokenVarTableColuna = tokenTypeMap.Where(x => x.Key == "VAR_TABLE_COLUNA").FirstOrDefault();
      var tokensTypeVarTableColuna = tokens.Where(x => x.Type == tokenVarTableColuna.Value).ToList();

      grupo = tokensTypeVarTableColuna.Select(x => new
      {
        tabela = x.Text.RemoveCaracter("@").SubstringWithIndexOf('.'),
        coluna = x.Text.RemoveCaracter("@").SubstringWithIndexOf('.', true).RemoveCaracter(".")
      })
          .GroupBy(u => u.tabela)
          .Select(y => new TabelaColuna()
          {
            Tabela = y.Select(y => y.tabela).Distinct().FirstOrDefault(),
            Coluna = y.Select(y => y.coluna).ToList()
          })
          .ToList();

      return grupo;
    }

    }
}