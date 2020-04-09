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
                Nome = "IPTU",
                SetorOrigem = SetorOrigem.Imobiliario,
            };

            Evento fatorG = new Evento
            {
                Id = 1,
                Nome = "FatorG",
                Formula = @"var dt1 = 14/02/2008;
var dt2 = _HOJE();
var dia = _DIA(dt2);
var mes = _MES(dt2);
var ano = _ANO(dt2);
var hora = _HORA(dt2);
var min = _MINUTO(dt2);
var switchTeste = -1;
parametro (min) {
	caso 08: {
		switchTeste = 0;
		parar;
	}
	caso 09: {
		switchTeste += 1;
		parar;
	}
	caso 10: {
		switchTeste = 2;
		parar;
	}
	padrao: switchTeste = -2;
}
var dtdf = _DATADIF(dt1, dt2, MES);
var percentualMinimo = 0.35;
var somase = _SOMASE(@FisicoOutros.Percentual, Percentual > percentualMinimo);
var countse = _CONTSE(@FisicoOutros, Percentual > percentualMinimo);
var abcv = _ARREDONDAR(_COALESCE(@FacesdaQuadra.LarguraRua, 3.989898), 2);
var index = 0;
var somaManual = 0.0;
var perc = 0.0;
var maior = 0.0;
var fisicoOutrosPercentual = 0;
enquanto (index < _CONT(@FisicoOutros)) {
	somaManual += _COALESCE(@FisicoOutros[index].Percentual, 0);
    fisicoOutrosPercentual = @FisicoOutros[index].Percentual;
	se fisicoOutrosPercentual >= maior {
		maior = @FisicoOutros[index].Percentual;
		perc *= _COALESCE(@FisicoOutros[index].Percentual, 0);
	} senao {
		parar;
		perc += 1;
	}
	index += 1;
} 
var tst5 = 2 ^ 3;
var tst6 = _RAIZ(11);
var somaFunc = _SOMA(@FisicoOutros.Percentual);
var maxFunc = _MAXIMO(@FisicoOutros.Percentual);

var valor = 1.0;
// Teste
var area = _COALESCE(@FisicoAreas[0].Area, @FacesdaQuadra.LarguraRua, @FisicoOutros[0].Percentual, 9);
var percentual = @FisicoOutros[0].Percentual;
se @Fisico.AreaEdificada > 0.0 {
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
                    se @Roteiro.FatorG > 5725.90 {
                        valor = 5725.90;
                    } senao {
                        valor = 3775.90;
                    }
                    // Retorna valor
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
                Formula = @"retorno @Fisico.AreaTerreno * 100;"
            };

            roteiro.Eventos.Add(fatorG);
            roteiro.Eventos.Add(fatorK);
            roteiro.Eventos.Add(vvt);
            roteiro.Eventos.Add(vvp);

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
                Nome = "createParametro2",
                Formula = @"retorno _PARAMETRO(""createParametro2"", 2016);"
            };

            Evento eventoParametroIntervalo = new Evento
            {
                Id = 71,
                Nome = "TabelaEmpregados",
                Formula = @"retorno _PARAMETROINTERVALO(""TabelaEmpregados"", ""12"");"
            };

            Evento eventoParametroCodigo = new Evento
            {
                Id = 72,
                Nome = "AjuizamentosFases",
                Formula = @"retorno _PARAMETROCODIGO(""AjuizamentosFases"", ""2"");"
            };

            roteiro.Eventos.Add(eventoParametroUnico);
            roteiro.Eventos.Add(eventoParametroIntervalo);
            roteiro.Eventos.Add(eventoParametroCodigo);

            Evento iptu = new Evento
            {
                Id = 60,
                Nome = "IPTU",
                Formula = @"
                    
                    var iptu_base = @Roteiro.vvt * @Roteiro.vvp - @Fisico.AreaEdificada;

                    se (@Roteiro.ILUMINAO > 0.0 && @Roteiro.ILUMINAO < 100.0) {
                        iptu_base = iptu_base + @Roteiro.ILUMINAO;
                    } senao {
                        iptu_base = iptu_base + @Roteiro.ILUMINAO + @Roteiro.ESGOTO;
                    }

                    se (@Roteiro.LIXO == 100.0 || @Roteiro.ESQUINA > 50.0) {
                        iptu_base = iptu_base - @Roteiro.REFORMADO;
                    } senao {
                        iptu_base = iptu_base + 5000;
                    }

                    retorno iptu_base * 0.5;"
            };

            roteiro.Eventos.Add(iptu);
            
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

            await Task.Delay(100);
            return roteiro;
        }
    }
}
