using Common.Extensions;
using Implementation;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Api
{
    public static class TesteLanguage
    {
        public static bool TesteFatorG(object entrada, object resultados, decimal? antlrResult)
        {
            var fisico = new
            {
                AreaEdificada = (entrada as IDictionary<string, GenericValueLanguage>)["@Fisico.AreaEdificada"].ToString().ToNullable<decimal>(),
                Testada = (entrada as IDictionary<string, GenericValueLanguage>)["@Fisico.Testada"].ToString().ToNullable<decimal>()
            };
            var fisicoOutros = ((entrada as IDictionary<string, GenericValueLanguage>).ContainsKey("@FisicoOutros") ?
                (entrada as IDictionary<string, GenericValueLanguage>)["@FisicoOutros"].AsDictionaryIntDictionaryStringGeneric() :
                new Dictionary<int, IDictionary<string, GenericValueLanguage>>())
                .Select(x => new
                {
                    Percentual = (x.Value as IDictionary<string, GenericValueLanguage>)["Percentual"].ToString().ToNullable<decimal>(),
                    Crc = (x.Value as IDictionary<string, GenericValueLanguage>)["Crc"].ToString().ToNullable<decimal>(),
                }).ToArray();

            var fisicoAreas = ((entrada as IDictionary<string, GenericValueLanguage>).ContainsKey("@FisicoAreas") ?
                (entrada as IDictionary<string, GenericValueLanguage>)["@FisicoAreas"].AsDictionaryIntDictionaryStringGeneric() :
                new Dictionary<int, IDictionary<string, GenericValueLanguage>>())
                .Select(x => new
                {
                    Area = (x.Value as IDictionary<string, GenericValueLanguage>)["Area"].ToString().ToNullable<decimal>(),
                }).ToArray();
            var facesdaQuadra = new
            {
                LarguraRua = (entrada as IDictionary<string, GenericValueLanguage>).ContainsKey("@FacesdaQuadra.LarguraRua") ? (entrada as IDictionary<string, GenericValueLanguage>)["@FacesdaQuadra.LarguraRua"].ToString().ToNullable<decimal>() : default,
            };

            var roteiro = new
            {
                Teste1 = (resultados as IDictionary<string, GenericValueLanguage>)["Teste1"].ToString().ToNullable<decimal>(),
                Teste2 = (resultados as IDictionary<string, GenericValueLanguage>)["Teste2"].ToString().ToNullable<decimal>(),
                Teste3 = (resultados as IDictionary<string, GenericValueLanguage>)["Teste3"].ToString().ToNullable<decimal>(),
                Teste4 = (resultados as IDictionary<string, GenericValueLanguage>)["Teste4"].ToString().ToNullable<decimal>(),
                TesteVarMemoryValue = (resultados as IDictionary<string, GenericValueLanguage>)["TesteVarMemoryValue"].AsDictionaryIntDictionaryStringGeneric()
                    .Select(x => new
                    {
                        Crc = (x.Value as IDictionary<string, GenericValueLanguage>)["Crc"].ToString().ToNullable<decimal>(),
                    }).ToArray(),
            };

            var dt1 = new DateTime(2008, 02, 14);
            var dt2 = DateTime.Today;
            var dia = dt2.Day;
            var mes = dt2.Month;
            var ano = dt2.Year;
            var dtdf = dt1.MonthDifference(dt2);
            var percentualMinimo = 100;
            var listaSomaCont = new Dictionary<int, ExpandoObject>();

            listaSomaCont.Add(0, new ExpandoObject());
            (listaSomaCont[0] as IDictionary<string, object>).Add("Percentual", 123);

            listaSomaCont.Add(1, new ExpandoObject());
            (listaSomaCont[1] as IDictionary<string, object>).Add("Percentual", 345);

            var somaseLista = roteiro.TesteVarMemoryValue.Where(x => x.Crc > percentualMinimo).Sum(x => x.Crc);
            var countseLista = roteiro.TesteVarMemoryValue.Where(x => x.Crc > percentualMinimo).Count();

            var somase = fisicoOutros.Where(x => x.Percentual > percentualMinimo).Sum(x => x.Percentual);
            var countse = fisicoOutros.Where(x => x.Percentual > percentualMinimo).Count();

            var index = 0;
            var somaManual = 0.0m;
            var perc = 0.0m;
            var maior = 0.0m;
            var listaValor = new Dictionary<int, ExpandoObject>();
            var tst5 = (decimal)Math.Pow(2, 3);

            var Teste1 = 0m;
            var Teste2 = 0m;
            var Teste3 = 0m;
            var Teste4 = 0m;
            while (index < fisicoOutros.Count())
            {

                Teste1 = Math.Round(roteiro.Teste1 ?? 1, 2);
                Teste2 = Math.Round(roteiro.Teste2 ?? 2, 2);
                Teste3 = Math.Round(roteiro.Teste3 ?? 3, 2);
                Teste4 = Math.Round(roteiro.Teste4 ?? 4, 2);

                listaValor.Add(index, new ExpandoObject());
                (listaValor[index] as IDictionary<string, object>).Add("TotalCaracteristica", Teste1 + Teste2 + Teste3 + Teste4);

                somaManual += fisicoOutros[index].Percentual ?? 0;
                if (maior < fisicoOutros[index].Percentual)
                {
                    maior = fisicoOutros[index].Percentual ?? 0;
                    perc *= (fisicoOutros[index].Percentual ?? 5);
                }
                else
                {
                    perc += 1;
                }
                (listaValor[index] as IDictionary<string, object>).Add("total", fisicoOutros[index].Crc);
                (listaValor[index] as IDictionary<string, object>).Add("totalSoma", (fisicoOutros[index].Crc ?? 0) * 3.989898m * tst5 + index);

                if (fisicoAreas.Count() > index)
                {
                    (listaValor[index] as IDictionary<string, object>).Add("PercentualCoalesce", fisicoAreas[index].Area ?? facesdaQuadra.LarguraRua ?? fisicoOutros[index].Percentual ?? 9);
                }
                else
                {
                    (listaValor[index] as IDictionary<string, object>).Add("PercentualCoalesce", facesdaQuadra.LarguraRua ?? fisicoOutros[index].Percentual ?? 9);
                }

                (listaValor[index] as IDictionary<string, object>).Add("Percentual", fisicoOutros[index].Percentual);
                index += 1;
            }

            var tst6 = Math.Sqrt(11);
            var somaFunc = fisicoOutros.Sum(x => x.Crc);
            var somaList = listaValor.Sum(x => (decimal)(x.Value as IDictionary<string, object>)["totalSoma"]);
            var maxFunc = roteiro.TesteVarMemoryValue.Max(x => x.Crc);
            //Aqui faz tal coisa
            var contList = listaValor.Count == 0 ? 0 : listaValor.Average(x => (decimal)(x.Value as IDictionary<string, object>)["totalSoma"]);
            var valor = 1.0m;

            if ((fisico.AreaEdificada ?? 0) > 0.0m)
            {
                valor = (fisico.AreaEdificada ?? 0) * 1.05m;
            }
            else
            {
                valor = (fisico.AreaEdificada ?? 0) * (fisico.Testada ?? 0);
            }
            var retorno = listaValor.Count == 0 ? 0 : listaValor.Average(x => (decimal)(x.Value as IDictionary<string, object>)["totalSoma"]);
            return retorno == antlrResult;
        }

        public static bool TesteoListMemoryValue(object entrada, decimal? antlrResult)
        {
            var fisicoOutros = ((entrada as IDictionary<string, GenericValueLanguage>).ContainsKey("@FisicoOutros") ?
                (entrada as IDictionary<string, GenericValueLanguage>)["@FisicoOutros"].AsDictionaryIntDictionaryStringGeneric() :
                new Dictionary<int, IDictionary<string, GenericValueLanguage>>())
                .Select(x => new
                {
                    Percentual = (x.Value as IDictionary<string, GenericValueLanguage>)["Percentual"].ToString().ToNullable<decimal>(),
                    Crc = (x.Value as IDictionary<string, GenericValueLanguage>)["Crc"].ToString().ToNullable<decimal>(),
                }).ToArray();

            var testeClaudio = fisicoOutros;
            var retorno = 10;
            return retorno == antlrResult;
        }

        public static bool TesteContLista(object resultados, decimal? antlr4Result)
        {
            var roteiro = new
            {
                FatorG = (resultados as IDictionary<string, GenericValueLanguage>)["FatorG"].ToString().ToNullable<decimal>(),
            };

            var fisicoOutros = 10;
            var retornoLista = new Dictionary<int, ExpandoObject>();
            var index = 0;

            retornoLista.Add(0, new ExpandoObject());
            (retornoLista[0] as IDictionary<string, object>).Add("Crc", roteiro.FatorG + 444);

            retornoLista.Add(1, new ExpandoObject());
            (retornoLista[1] as IDictionary<string, object>).Add("Crc", roteiro.FatorG + 444 * 2);
            var retorno = retornoLista.Count;

            return retorno == antlr4Result;
        }

        public static bool RetornoLista(IDictionary<int, IDictionary<string, GenericValueLanguage>> antlr4Result)
        {
            var somaLista = new Dictionary<int, IDictionary<string, GenericValueLanguage>>();

            somaLista.Add(0, new Dictionary<string, GenericValueLanguage>());
            somaLista[0].Add("Valor", new GenericValueLanguage(10));

            somaLista.Add(1, new Dictionary<string, GenericValueLanguage>());
            somaLista[1].Add("Valor", new GenericValueLanguage(50));

            somaLista.Add(3, new Dictionary<string, GenericValueLanguage>());
            somaLista[3].Add("Valor", new GenericValueLanguage(10));

            somaLista[1].Add("Claudio", new GenericValueLanguage(10));

            var retorno = somaLista[0];

            return (decimal)retorno["Valor"] == (decimal)antlr4Result[0]["Valor"];
        }

        public static bool TesteRetornoVarMemoryValue(object entrada, object resultados, decimal? antlrResult)
        {
            var fisico = new
            {
                CrcProprietario = (entrada as IDictionary<string, GenericValueLanguage>).ContainsKey("@Fisico.CrcProprietario") ? (entrada as IDictionary<string, GenericValueLanguage>)["@Fisico.CrcProprietario"].ToString().ToNullable<int>() : default,
            };
            var roteiro = new
            {
                TesteVarMemoryValue = (resultados as IDictionary<string, GenericValueLanguage>)["TesteVarMemoryValue"].AsDictionaryIntDictionaryStringGeneric()
                    .Select(x => new
                    {
                        Crc = (x.Value as IDictionary<string, GenericValueLanguage>)["Crc"].ToString().ToNullable<decimal>(),
                    }).ToArray(),
            };

            var teste = roteiro.TesteVarMemoryValue[0].Crc + fisico.CrcProprietario;
            var retorno = teste;

            return retorno == antlrResult;
        }

        public static bool TesteCaracteristica(decimal? caracteristica, decimal? antlrResult)
        {
            return caracteristica == antlrResult;
        }

        public static bool UsandoFatorG(object resultados, decimal? antlrResult)
        {
            var roteiro = new
            {
                FatorG = (resultados as IDictionary<string, GenericValueLanguage>)["FatorG"].ToString().ToNullable<decimal>(),
                RetornoLista = (resultados as IDictionary<string, GenericValueLanguage>)["Retorno_Lista"].AsDictionaryIntDictionaryStringGeneric(),
            };

            var teste = roteiro.FatorG;
            var retorno = roteiro.RetornoLista.Count;


            return retorno == antlrResult;
        }

        public static bool TesteFatorK(decimal entrada, decimal antlrResult)
        {
            var valor = 1.0m;
            if (entrada > 5725.90m)
            {
                valor = 5725.90m;
            }
            else
            {
                valor = 3775.90m;
            }
            // Retorna valor
            return valor == antlrResult;
        }

        public static bool TesteVVT(decimal areaTerreno, decimal fatorG, decimal fatorK, decimal antlrResult)
        {
            var result = fatorG * fatorK + areaTerreno;
            return result == antlrResult;
        }

        public static bool TesteVVP(decimal areaTerreno, decimal antlrResult)
        {
            var result = areaTerreno * 100;
            return result == antlrResult;
        }

        public static bool TesteIPTU(object fisicoEntrada, object resultadosEntrada, decimal antlrResult)
        {
            var fisico = new
            {
                AreaEdificada = decimal.Parse((fisicoEntrada as IDictionary<string, object>)["AreaEdificada"].ToString())
            };
            var roteiro = new
            {
                VVT = decimal.Parse((resultadosEntrada as IDictionary<string, object>)["vvt"].ToString()),
                VVP = decimal.Parse((resultadosEntrada as IDictionary<string, object>)["vvp"].ToString()),
                ILUMINAO = (resultadosEntrada as IDictionary<string, object>).TryGetValue("ILUMINAO", out object iluminacao) ? iluminacao.ToString().ToNullable<decimal>() : null,
                LIXO = (resultadosEntrada as IDictionary<string, object>).TryGetValue("LIXO", out object lixo) ? lixo.ToString().ToNullable<decimal>() : null,
                ESQUINA = (resultadosEntrada as IDictionary<string, object>).TryGetValue("ESQUINA", out object esquina) ? esquina.ToString().ToNullable<decimal>() : null,
                REFORMADO = (resultadosEntrada as IDictionary<string, object>).TryGetValue("REFORMADO", out object reformado) ? reformado.ToString().ToNullable<decimal>() : null,
                ESGOTO = (resultadosEntrada as IDictionary<string, object>).TryGetValue("ESGOTO", out object esgoto) ? esgoto.ToString().ToNullable<decimal>() : null,
            };

            var iptu_base = roteiro.VVT * roteiro.VVP - fisico.AreaEdificada;

            if (roteiro.ILUMINAO > 0.0m && roteiro.ILUMINAO < 100.0m)
            {
                iptu_base = iptu_base + (roteiro.ILUMINAO ?? 0.0m);
            }
            else
            {
                iptu_base = iptu_base + (roteiro.ILUMINAO ?? 0.0m) + (roteiro.ESGOTO ?? 0.0m);
            }

            if (roteiro.LIXO == 100.0m || roteiro.ESQUINA > 50.0m)
            {
                iptu_base = iptu_base - (roteiro.REFORMADO ?? 0.0m);
            }
            else
            {
                iptu_base = iptu_base + 5000;
            }

            return iptu_base * 0.5m == antlrResult;
        }
    }
}
