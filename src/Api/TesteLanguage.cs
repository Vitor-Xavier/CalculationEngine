using System;
using System.Collections.Generic;
using Implementation;

namespace Api
{
    public static class TesteLanguage
    {
        public static bool TesteFatorG(object entrada, decimal antlrResult)
        {
            var fisico = new
            {
                AreaEdificada = (entrada as IDictionary<string, object>)["AreaEdificada"].ToString().ToNullable<decimal>(),
                Testada = (entrada as IDictionary<string, object>)["Testada"].ToString().ToNullable<decimal>()
            };
            var fisicoOutros = (entrada as IDictionary<string, object>).ContainsKey("FisicoOutros") ?
            new
            {
                Percentual = (((entrada as IDictionary<string, object>)["FisicoOutros"] as object[])[0] as IDictionary<string, object>)["Percentual"]?.ToString().ToNullable<decimal>(),
            } : new { Percentual = (decimal?)null };
            var fisicoAreas = (entrada as IDictionary<string, object>).ContainsKey("FisicoAreas") ?
            new
            {
                Area = (((entrada as IDictionary<string, object>)["FisicoAreas"] as object[])[0] as IDictionary<string, object>)["Area"]?.ToString().ToNullable<decimal>(),
            } : new { Area = (decimal?)null };
            var faceQuadra = new
            {
                LarguraRua = ((entrada as IDictionary<string, object>)["FacesdaQuadra"] as IDictionary<string, object>)["LarguraRua"]?.ToString().ToNullable<decimal>(),
            };

            // Teste
            var area = (fisicoAreas.Area ?? faceQuadra.LarguraRua ?? fisicoOutros.Percentual ?? 9.0m);
            var percentual = fisicoOutros.Percentual;
            var valor = 0m;
            if (fisico.AreaEdificada > 0.0m)
            {
                valor = Math.Round((fisico.AreaEdificada ?? 0.0m) * 1.05m, LanguageDefault.DecimalPlaces);
            }
            else
            {
                valor = Math.Round((fisico.AreaEdificada ?? 0.0m) * (fisico.Testada ?? 0.0m), LanguageDefault.DecimalPlaces);
            }

            return Math.Round(valor * area, 4) == antlrResult;
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
            var result = Math.Round(Math.Round(fatorG * fatorK, 4) + areaTerreno, LanguageDefault.DecimalPlaces);
            return result == Math.Round(antlrResult, LanguageDefault.DecimalPlaces);
        }

        public static bool TesteVVP(decimal areaTerreno, decimal antlrResult)
        {
            var result = Math.Round(areaTerreno * 100, LanguageDefault.DecimalPlaces);
            return result == Math.Round(antlrResult, LanguageDefault.DecimalPlaces);
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

            var iptu_base = Math.Round(Math.Round(roteiro.VVT * roteiro.VVP, 4) - fisico.AreaEdificada, LanguageDefault.DecimalPlaces);

            if (roteiro.ILUMINAO > 0.0m && roteiro.ILUMINAO < 100.0m)
            {
                iptu_base = Math.Round(iptu_base + (roteiro.ILUMINAO ?? 0.0m), LanguageDefault.DecimalPlaces);
            }
            else
            {
                iptu_base = Math.Round(Math.Round(iptu_base + (roteiro.ILUMINAO ?? 0.0m), 4) + (roteiro.ESGOTO ?? 0.0m), LanguageDefault.DecimalPlaces);
            }

            if (roteiro.LIXO == 100.0m || roteiro.ESQUINA > 50.0m)
            {
                iptu_base = Math.Round(iptu_base - (roteiro.REFORMADO ?? 0.0m), LanguageDefault.DecimalPlaces);
            }
            else
            {
                iptu_base = Math.Round(iptu_base + 5000, LanguageDefault.DecimalPlaces);
            }

            return Math.Round(iptu_base * 0.5m, 4) == antlrResult;
        }
    }
}
