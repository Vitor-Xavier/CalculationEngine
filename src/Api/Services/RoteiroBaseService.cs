using Api.Dto;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api.Services
{


  public class RoteiroBaseService
  {

    private readonly string[] arrayCaracteristica =  {"\"Planta de Valor Construção\"",
                "\"Planta de Valor Terreno\"",
                "\"CARTORIO\"",
                "\"TAXA INCÊNDIO\"",
                "\"TAXA VIAÇÃO\"",
                "\"ZONA\"",
                "\"CARAC. DO TRECHO\"",
                "\"CLASSE OCUPAÇÃO\"",
                "\"CATEGORIA\"",
                "\"TIPO\"",
                "\"TAXA SANITÁRIA\"",
                "\"REFLORESTAMENTO\"",
                "\"REFORMADO\"",
                "\"VIZINHANÇA DE CORREGO\"",
                "\"DESVIO FERROVIÁRIO\"",
                "\"LIXO\"",
                "\"MURO\"",
                "\"CALÇADA\"",
                "\"ILUMINAÇÃO\"",
                "\"ESGOTO\"",
                "\"ÁGUA\"",
                "\"SARJETA\"",
                "\"ESQUINA\"",
                "\"TOPOGRAFIA\"",
                "\"PAVIMENTAÇÃO\"",
                "\"MEIO FIO\"",
                "\"ESCOLA\""};

    public async Task<Roteiro> GetRoteiro()
    {
      Roteiro roteiro = new Roteiro
      {
        RoteiroId = 1,
        Nome = "Base",
        SetorOrigem = SetorOrigem.Imobiliario,
      };


      Evento caracteristica = new Evento
      {
        Id = 60,
        Nome = "CARACTERISTICA",
        Formula = string.Format("retorno _CARACTERISTICA( {0},{1},{2},{3});"
            , "\"ESGOTO\""
            , "\"01\""
            , "\"Valor\""
            , 2019)
      };

      roteiro.Eventos.Add(caracteristica);

      string FisicoCaracteristicas = "\"FisicoCaracteristicas\"";
      string DescricaoCaracteristica = "\"Planta de Valor Construção\"";
      string ColunaCaracteristica = "\"IdFisico\"";
      int ExercicioCaracteristica = 2019;
      string Codigo = "\"01\"";
      string ValorFatorCaracteristica = "\"Valor\"";

      List<Evento> evt = new List<Evento>();
      for (var i = 0; i < 27; i++)
      {



        DescricaoCaracteristica = arrayCaracteristica[i];
        string Nome = Regex.Replace(DescricaoCaracteristica, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);

        Evento eventoBusca = new Evento
        {
          Id = 5,
          Nome = Nome,
          //Formula = string.Format("var teste = 1.0; var teste_claudio{5} = _CARACTERISTICA({1},{6},{4}, {3}); retorno _CARACTERISTICATABELA({0},{1},{2},{3}, {4});"
          Formula = string.Format("retorno _CARACTERISTICATABELA({0},{1},{2},{3},{4});"
                  , FisicoCaracteristicas
                  , DescricaoCaracteristica
                  , ColunaCaracteristica
                  , ExercicioCaracteristica
                  , ValorFatorCaracteristica, i, Codigo)
        };
        evt.Add(eventoBusca);
      };

      //evt.ForEach(item => roteiro.Eventos.Add(item));

      Evento basicoListaAtividade = new Evento
      {
        Id = 60,
        Nome = "basicoListaAtividade",
        Formula = string.Format("lista listaAtividade = []; listaAtividade[0].Nome = \"Claudio\"; retorno @FisicoOutros[0].Crc;")
      };

      Evento basicoListaInicioColchete = new Evento
      {
        Id = 60,
        Nome = "basicoListaInicioColchete",
        Formula = string.Format("lista listaInicioColchete = [\"BaseDoisValue01\",\"BaseDoisValue02\", 12]; listaInicioColchete[3] = \"Valor Novo\"; listaInicioColchete[6] = 100;var oi = 10; retorno @Fisico.Origem;")
      };

      Evento basicoListaInicioColcheteProperty = new Evento
      {
        Id = 60,
        Nome = "basicoListaInicioColcheteProperty",
        //Formula = string.Format("lista listaInicioColcheteProperty[0].Nome = 10; listaInicioColcheteProperty[1].Nome = 20; listaInicioColcheteProperty[3].Nome = \"Spinelli\";listaInicioColcheteProperty[0].Sobrenome = \"Spinelli\";listaInicioColcheteProperty[1].Nome = 80; listaInicioColcheteProperty[1].Sobrenome = \"Clemente\"; retorno _MINIMO(listaInicioColcheteProperty.Nome);")
        Formula = string.Format("lista listaInicioColcheteProperty = []; listaInicioColcheteProperty[0].Nome = 10; listaInicioColcheteProperty[1].Nome = 20; listaInicioColcheteProperty[3].Nome = 4;listaInicioColcheteProperty[0].Sobrenome = \"Spinelli\";listaInicioColcheteProperty[1].Nome = 80; listaInicioColcheteProperty[1].Sobrenome = \"Clemente\"; retorno listaInicioColcheteProperty;")
      };

      Evento basicoDois = new Evento
      {
        Id = 60,
        Nome = "BaseDois",
        Formula = string.Format("lista BaseDois = [\"BaseDoisValue01\",\"BaseDoisValue02\", 12]; BaseDois[0] = 40 + BaseDois[2]; var valor = @Roteiro.basicoListaInicioColcheteProperty[1].Sobrenome; retorno @Roteiro.basicoListaInicioColcheteProperty[0].Sobrenome;")
      };

      Evento basicoTres = new Evento
      {
        Id = 60,
        Nome = "BaseTres",
        Formula = string.Format("lista BaseTres = []; BaseTres[0].PropriedadeTres = @Roteiro.BaseDois[2]; retorno BaseTres[0].PropriedadeTres;")
      };


      Evento basicoQuarto = new Evento
      {
        Id = 60,
        Nome = "BaseQuarto",
        Formula = string.Format("lista teste = _ATIVIDADETABELA(\"CCMAtividades\", \"SERVIÇOS116\", \"ccm\", 2019, \"IdAtividade\", [\"Atividades.VlrAtividade\",\"Atividades.Aliquota\",\"AtividadesVlrs.Valor\",\"AtividadesVlrs.DtInicio\"]); retorno teste; ")
      };

      Evento basicoCinco = new Evento
      {
        Id = 1,
        Nome = "BaseCinco",
        Formula = @"var testeTT = 1; var origem = @Fisico.CrcProprietario; var crc = @FacesdaQuadra.IdCep; retorno @Roteiro.BaseQuarto[0].Valor;"
      };

      string formulaTeste = "lista teste = _ATIVIDADETABELA(\"CCMAtividades\", \"SERVIÇOS116\", \"ccm\", 2019, \"IdAtividade\", [\"Atividades.VlrAtividade\",\"Atividades.Aliquota\",\"AtividadesVlrs.Valor\",\"AtividadesVlrs.DtInicio\"]);";
      Evento basicoSeis = new Evento
      {
        Id = 1,
        Nome = "BaseSeis",
        Formula = "var index = 0; var somaManual = 0.0; lista claudio = [100,200,300];lista BaseSeis = _ATIVIDADETABELA(\"CCMAtividades\", \"SERVIÇOS116\", \"ccm\", 2019, \"IdAtividade\", [\"Atividades.VlrAtividade\",\"Atividades.Aliquota\",\"AtividadesVlrs.Valor\",\"AtividadesVlrs.DtInicio\"]); enquanto (index < _CONT(BaseSeis)) { somaManual += index + BaseSeis[index].Valor; index +=  1; } retorno somaManual;"
      };

      Evento basicoSete = new Evento
      {
        Id = 1,
        Nome = "BaseSete",
        Formula = @"lista listaCompleta = []; listaCompleta[0].teste = 10;listaCompleta[0].testeDois = 20;listaCompleta[1].teste = 30; var somaTotal = listaCompleta[0].teste + listaCompleta[0].testeDois + listaCompleta[1].teste; retorno _CONT(listaCompleta);"
      };



      // roteiro.Eventos.Add(basicoListaAtividade);
      // roteiro.Eventos.Add(basicoListaInicioColchete);
      // roteiro.Eventos.Add(basicoListaInicioColcheteProperty);
      // roteiro.Eventos.Add(basicoDois);
      // roteiro.Eventos.Add(basicoTres);
      roteiro.Eventos.Add(basicoQuarto);
      roteiro.Eventos.Add(basicoCinco);
      // roteiro.Eventos.Add(basicoSeis);
      // roteiro.Eventos.Add(basicoSete);



      await Task.Delay(100);
      return roteiro;
    }
  }
}

