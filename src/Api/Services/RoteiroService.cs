using Api.Dto;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api.Services
{
    public class RoteiroService
    {
        string[] arrayCaracteristica =  {"\"Planta de Valor Construção\"",
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
                Formula = @"
                    var valor = 1.0;
                    // Teste
                    var area = _COALESCE(@FisicoAreas[0].Area, @FacesdaQuadra.LarguraRua, @FisicoOutros[0].Percentual, 9);
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

            roteiro.Eventos.Add(fatorG);
            roteiro.Eventos.Add(fatorK);
            roteiro.Eventos.Add(vvt);
            roteiro.Eventos.Add(vvp);

            evt.ForEach(item => roteiro.Eventos.Add(item));


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

            await Task.Delay(100);
            return roteiro;
        }
    }
}
