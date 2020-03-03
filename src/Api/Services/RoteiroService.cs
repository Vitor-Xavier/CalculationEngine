using Api.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services
{
  public class RoteiroService
  {
    public async Task<Roteiro> GetRoteiro()
    {
      Roteiro roteiro = new Roteiro
      {
        Id = 1,
        NomeRoteiro = "IPTU",
        SetorOrigem = SetorOrigem.Imobiliario,
        Eventos = new List<Evento>()
      };

      Evento fatorG = new Evento
      {
        Id = 1,
        Nome = "FatorG",
        Formula = @"
                    var valor = 1.0;
                    var area = _COALESCE(@FisicoAreas[0].Area, @FisicoOutros[0].Percentual, 9.0);
                    var percentual = @FisicoOutros[0].Percentual;
                    se (@Fisico.AreaEdificada > 0.0) {
                        valor = @Fisico.AreaEdificada * 1.05;
                    } senao {
                        valor = @Fisico.AreaEdificada * @Fisico.Testada;
                    }
                    retorno valor * area;"
      };

      Evento fatorK = new Evento
      {
        Id = 2,
        Nome = "FatorK",
        Formula = @"
                    var valor = 1.0;
                    se (@Roteiro.FatorG > 5725.90) {
                        valor = 5725.90;
                    } senao {
                        valor = 3775.90;
                    }
                    retorno valor;"
      };

      Evento vvt = new Evento
      {
        Id = 3,
        Nome = "vvt",
        Formula = @"retorno @Roteiro.FatorG * @Roteiro.FatorK + @Fisico.AreaTerreno;"
      };

      Evento vvp = new Evento
      {
        Id = 4,
        Nome = "vvp",
        Formula = @"retorno @Fisico.AreaTerreno * 100.0;"
      };


      string FisicoCaracteristicas = "\"FisicoCaracteristicas\"";
      string DescricaoCaracteristica = "\"AGUA\"";
      string ColunaCaracteristica = "\"IdFisico\"";
      int ExercicioCaracteristica = 2019;
      string ValorFatorCaracteristica = "\"DiferenteTabela\"";

      Evento iptu = new Evento
      {
        Id = 5,
        Nome = "IPTU",
        Formula = string.Format("var teste = 1.0; _BuscarCaracteristica({0},{1},{2},{3},{4}); retorno 15;"
        , FisicoCaracteristicas
        , DescricaoCaracteristica
        , ColunaCaracteristica
        , ExercicioCaracteristica
        , ValorFatorCaracteristica)
      };

      FisicoCaracteristicas = "\"FisicoCaracteristicas\"";
      DescricaoCaracteristica = "\"ESCOLA\"";
      ColunaCaracteristica = "\"IdFisico\"";
      ExercicioCaracteristica = 2019;
      ValorFatorCaracteristica = "\"DiferenteTabela\"";

      Evento iptu2 = new Evento
      {
        Id = 5,
        Nome = "IPTU2",
        Formula = string.Format("var teste = 1.0; _BuscarCaracteristica({0},{1},{2},{3}, {4}); retorno 15;"
        , FisicoCaracteristicas
        , DescricaoCaracteristica
        , ColunaCaracteristica
        , ExercicioCaracteristica
        , ValorFatorCaracteristica)
      };


      FisicoCaracteristicas = "\"FisicoCaracteristicas\"";
      DescricaoCaracteristica = "\" ESGOTO\"";
      ColunaCaracteristica = "\"IdFisico\"";
      ExercicioCaracteristica = 2019;
      ValorFatorCaracteristica = "\"DiferenteTabela\"";

      Evento iptu3 = new Evento
      {
        Id = 5,
        Nome = "IPTU3",
        Formula = string.Format("var teste = 1.0; _BuscarCaracteristica({0},{1},{2},{3}, {4}); retorno 15;"
        , FisicoCaracteristicas
        , DescricaoCaracteristica
        , ColunaCaracteristica
        , ExercicioCaracteristica
        , ValorFatorCaracteristica)
      };

      FisicoCaracteristicas = "\"FisicoCaracteristicas\"";
      DescricaoCaracteristica = "\" CARTORIO Teste\"";
      ColunaCaracteristica = "\"IdFisico\"";
      ExercicioCaracteristica = 2019;
      ValorFatorCaracteristica = "\"DiferenteTabela\"";

      Evento iptu4 = new Evento
      {
        Id = 5,
        Nome = "IPTU4",
        Formula = string.Format("var teste = 1.0; _BuscarCaracteristica({0},{1},{2},{3}, {4}); retorno 15;"
        , FisicoCaracteristicas
        , DescricaoCaracteristica
        , ColunaCaracteristica
        , ExercicioCaracteristica
        , ValorFatorCaracteristica)
      };


      roteiro.Eventos.Add(fatorG);
      roteiro.Eventos.Add(fatorK);
      roteiro.Eventos.Add(vvt);
      roteiro.Eventos.Add(vvp);
      roteiro.Eventos.Add(iptu);
      roteiro.Eventos.Add(iptu2);
      roteiro.Eventos.Add(iptu3);

      await Task.Delay(100);
      return roteiro;
    }
  }
}
