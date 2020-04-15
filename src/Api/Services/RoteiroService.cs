using Api.Dto;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api.Services
{
  public class RoteiroService
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

      Evento AtividadeTabelaRetorno = new Evento
      {
        Id = 1,
        Nome = "AtividadeTabelaRetorno",
        Formula = string.Format("retorno _ATIVIDADETABELA(\"CCMAtividades\", \"SERVIÇOS116\", \"ccm\", 2019, \"IdAtividade\", [\"Atividades.VlrAtividade\",\"Atividades.Aliquota\",\"AtividadesVlrs.Valor\",\"AtividadesVlrs.DtInicio\"]);")
      };
      roteiro.Eventos.Add(AtividadeTabelaRetorno);

      Evento AtividadeTabelaRetornoTeste = new Evento
      {
        Id = 1,
        Nome = "AtividadeTabelaRetornoTeste",
        Formula = string.Format("lista listaTeste= _ATIVIDADETABELA(\"CCMAtividades\", \"SERVIÇOS116\", \"ccm\", 2019, \"IdAtividade\", [\"Atividades.VlrAtividade\",\"Atividades.Aliquota\",\"AtividadesVlrs.Valor\",\"AtividadesVlrs.DtInicio\"]); retorno _ARREDONDAR(_SOMASE(listaTeste.Valor, Valor > 100),6); ")
      };
      roteiro.Eventos.Add(AtividadeTabelaRetornoTeste);

      Evento Soma_Retorno_AtividadeTabelaRetorno = new Evento
      {
        Id = 1,
        Nome = "Soma_Retorno_AtividadeTabelaRetorno",
        Formula = string.Format("retorno _SOMASE(@Roteiro.AtividadeTabelaRetorno.Valor, Valor > 100);")
      };
      roteiro.Eventos.Add(Soma_Retorno_AtividadeTabelaRetorno);

      Evento Soma_AtividadeTabelaRetorno = new Evento
      {
        Id = 1,
        Nome = "Soma_AtividadeTabelaRetorno",
        Formula = string.Format("lista Soma_AtividadeTabelaRetorno = _ATIVIDADETABELA(\"CCMAtividades\", \"SERVIÇOS116\", \"ccm\", 2019, \"IdAtividade\", [\"Atividades.VlrAtividade\",\"Atividades.Aliquota\",\"AtividadesVlrs.Valor\",\"AtividadesVlrs.DtInicio\"]); retorno _ARREDONDAR(_SOMA(Soma_AtividadeTabelaRetorno.Valor),2);")
      };
      roteiro.Eventos.Add(Soma_AtividadeTabelaRetorno);

      Evento Soma_da_Soma_Retorno_AtividadeTabelaRetorno = new Evento
      {
        Id = 1,
        Nome = "Soma_da_Soma_Retorno_AtividadeTabelaRetorno",
        Formula = string.Format("retorno _SOMA(@Roteiro.Soma_Retorno_AtividadeTabelaRetorno);")
      };
      roteiro.Eventos.Add(Soma_da_Soma_Retorno_AtividadeTabelaRetorno);

      Evento Soma_Lista = new Evento
      {
        Id = 1,
        Nome = "Soma_Lista",
        Formula = string.Format("lista somaLista =[]; somaLista[0].Valor = 10; somaLista[1].Valor = 50; somaLista[3].Valor = 10; somaLista[1].Claudio = 10; retorno _SOMA(somaLista.Valor);")
      };
      roteiro.Eventos.Add(Soma_Lista);

      Evento Retorno_Lista = new Evento
      {
        Id = 1,
        Nome = "Retorno_Lista",
        Formula = string.Format("lista somaLista =[]; somaLista[0].Valor = 10; somaLista[1].Valor = 50; somaLista[3].Valor = 10; somaLista[1].Claudio = 10; retorno somaLista[0];")
      };
      roteiro.Eventos.Add(Retorno_Lista);

      Evento Soma_Retorno_Lista = new Evento
      {
        Id = 1,
        Nome = "Soma_Retorno_Lista",
        Formula = string.Format("retorno _SOMA(@Roteiro.Retorno_Lista.Valor);")
      };
      roteiro.Eventos.Add(Soma_Retorno_Lista);

      Evento TesteVarMemoryValue = new Evento
      {
        Id = 1,
        Nome = "TesteVarMemoryValue",
        Formula = string.Format("var fisicoOutros = 10; lista retornoLista = []; var index= 0; retornoLista[0].Crc = 666; retornoLista[1].Crc = 888;  retorno retornoLista;")
      };
      roteiro.Eventos.Add(TesteVarMemoryValue);

      Evento TesteRetornoVarMemoryValue = new Evento
      {
        Id = 1,
        Nome = "TesteRetornoVarMemoryValue",
        Formula = string.Format("var teste = @Roteiro.TesteVarMemoryValue[0].Crc + @Fisico.CrcProprietario; retorno teste; ")
      };
      roteiro.Eventos.Add(TesteRetornoVarMemoryValue);

      Evento TesteoListMemoryValue = new Evento
      {
        Id = 1,
        Nome = "TesteoListMemoryValue",
        Formula = string.Format("lista testeClaudio = @FisicoOutros; retorno 10;")
      };
      roteiro.Eventos.Add(TesteoListMemoryValue);

      Evento TesteRetornoListMemoryValue = new Evento
      {
        Id = 1,
        Nome = "TesteRetornoListMemoryValue",
        Formula = string.Format("var teste = @Roteiro.TesteoListMemoryValue; retorno teste; ")
      };
      roteiro.Eventos.Add(TesteRetornoListMemoryValue);

      Evento caracteristica = new Evento
      {
        Id = 60,
        Nome = "CARACTERISTICA",
        Formula = string.Format("retorno _ARREDONDAR(_CARACTERISTICA( {0},{1},{2},{3}),2);"
            , "\"ESGOTO\""
            , "\"01\""
            , "\"Valor\""
            , 2019)
      };

      roteiro.Eventos.Add(caracteristica);

      Evento fatorG = new Evento
      {
        Id = 1,
        Nome = "FatorG",
        Formula = @"
                      var dt1 = 14/02/2008;
                     var dt2 = _HOJE();
                     var dia = _DIA(dt2);
                     var mes = _MES(dt2);
                     var ano = _ANO(dt2);
                     var dtdf = _DATADIF(dt1, dt2, MES);
                     var percentualMinimo = 100;
                     lista listaSomaCont = [];

                     listaSomaCont[0].Percentual = 123;
                     listaSomaCont[1].Percentual = 345;

                     var somaseLista = _SOMASE(@Roteiro.TesteVarMemoryValue.Crc, Crc > percentualMinimo);
                     var countseLista = _CONTSE(@Roteiro.TesteVarMemoryValue, Crc > percentualMinimo);

                     var somase = _SOMASE(@FisicoOutros.Percentual, Percentual > percentualMinimo);
                     var countse = _CONTSE(@FisicoOutros, Percentual > percentualMinimo);
                     var abcv = _ARREDONDAR(_COALESCE(@FisicoOutros[0].Crc, 3.989898), 2);
                     var index = 0;
                     var somaManual = 0.0;
                     var perc = 0.0;
                     var maior = 0.0;
                     lista listaValor = [];
                     var tst5 = 2 ^ 3;
                     enquanto (index < _CONT(@FisicoOutros)) {
                         somaManual += _COALESCE(@FisicoOutros[index].Percentual, 0);
                         se (maior < @FisicoOutros[index].Percentual) {
                             maior = @FisicoOutros[index].Percentual;
                             perc *= _COALESCE(@FisicoOutros[index].Percentual, 5);
                         } senao {
                             perc += 1;
                         }
                          listaValor[index].total = @FisicoOutros[index].Crc;
                          listaValor[index].totalSoma = @FisicoOutros[index].Crc * 3.989898 * tst5 + index;
                         index +=  1;
                     }
                     
                     var tst6 = _RAIZ(11);
                     var somaFunc = _SOMA(@FisicoOutros.Crc);
                     var somaList = _SOMA(listaValor.totalSoma);
                     var maxFunc = _MAXIMO(@Roteiro.TesteVarMemoryValue.Crc);
                     //Aqui faz tal coisa
                     var contList = _MEDIA(listaValor.totalSoma);
                    var valor = 1.0;
                     
                    var area = _COALESCE(@FisicoAreas[0].Area, @FacesdaQuadra.LarguraRua, @FisicoOutros[0].Percentual, 9);
                    var percentual = @FisicoOutros[0].Percentual;
                    se (@Fisico.AreaEdificada > 0.0) {
                        valor = @Fisico.AreaEdificada * 1.05;
                    } senao {
                        valor = @Fisico.AreaEdificada * @Fisico.Testada;
                    }
                    retorno _MEDIA(@Roteiro.CARACTERISTICA);
                     "
      };

      roteiro.Eventos.Add(fatorG);

      Evento UsandoFatorG = new Evento
      {
        Id = 1,
        Nome = "UsandoFatorG",
        Formula = string.Format("var teste = @Roteiro.FatorG[0].total; retorno _CONT(@Roteiro.Retorno_Lista); ")
      };
      roteiro.Eventos.Add(UsandoFatorG);

      Evento TesteContLista = new Evento
      {
        Id = 1,
        Nome = "TesteContLista",
        Formula = string.Format("var fisicoOutros = 10; lista retornoLista = []; var index= 0; retornoLista[0].Crc = @FisicoOutros[0].Crc; retornoLista[1].Crc = @FisicoOutros[1].Crc*2;  retorno _CONT(retornoLista);")
      };
      roteiro.Eventos.Add(TesteContLista);

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

      evt.ForEach(item => roteiro.Eventos.Add(item));

      Evento eventoParametroUnico = new Evento
      {
        Id = 70,
        Nome = "eventoParametroUnico",
        Formula = @"lista parametro = _PARAMETRO(""ReferenciaBaixaFebrabanMovCxIt"", 2019); retorno parametro;"
      };


      roteiro.Eventos.Add(eventoParametroUnico);

      Evento eventoParametroUnico2 = new Evento
      {
        Id = 70,
        Nome = "eventoParametroUnico2",
        Formula = @"retorno @Roteiro.eventoParametroUnico[0].Valor;"
      };
      roteiro.Eventos.Add(eventoParametroUnico2);


      Evento eventoParametroUnico3 = new Evento
      {
        Id = 70,
        Nome = "eventoParametroUnico3",
        Formula = @"retorno _SOMA(@Roteiro.eventoParametroUnico.Valor);"
      };
      roteiro.Eventos.Add(eventoParametroUnico3);

      await Task.Delay(100);
      return roteiro;
    }
  }
}
