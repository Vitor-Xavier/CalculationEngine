using System;
using System.Collections.Generic;

namespace Api
{
    public static class TesteLanguage
    {
        public static bool TesteFatorG(object entrada, double antlrResult)
        {
            var fisico = new
            {
                AreaEdificada = (entrada as IDictionary<string, object>)["AreaEdificada"].ToString().ToNullable<double>(),
                Testada = (entrada as IDictionary<string, object>)["Testada"].ToString().ToNullable<double>()
            };
            var fisicoOutros = (entrada as IDictionary<string, object>).ContainsKey("FisicoOutros") ?
            new
            {
                Percentual = (((entrada as IDictionary<string, object>)["FisicoOutros"] as object[])[0] as IDictionary<string, object>)["Percentual"]?.ToString().ToNullable<double>(),
            } : new { Percentual = (double?)null };
            var fisicoAreas = (entrada as IDictionary<string, object>).ContainsKey("FisicoAreas") ?
            new
            {
                Area = (((entrada as IDictionary<string, object>)["FisicoAreas"] as object[])[0] as IDictionary<string, object>)["Area"]?.ToString().ToNullable<double>(),
            } : new { Area = (double?)null };
            var faceQuadra = new
            {
                LarguraRua = ((entrada as IDictionary<string, object>)["FacesdaQuadra"] as IDictionary<string, object>)["LarguraRua"]?.ToString().ToNullable<double>(),
            };

            var valor = 1.0;
            // Teste
            var area = (fisicoAreas.Area ?? faceQuadra.LarguraRua ?? fisicoOutros.Percentual ?? 9.0);
            var percentual = fisicoOutros.Percentual;
            if (fisico.AreaEdificada > 0.0)
            {
                valor = Math.Round((fisico.AreaEdificada ?? 0.0) * 1.05, 4);
            }
            else
            {
                valor = Math.Round((fisico.AreaEdificada ?? 0.0) * (fisico.Testada ?? 0.0), 4);
            }

            return Math.Round(valor * area, 4) == antlrResult;
        }

        public static bool TesteFatorK(double entrada, double antlrResult)
        {
            var valor = 1.0;
            if (entrada > 5725.90)
            {
                valor = 5725.90;
            }
            else
            {
                valor = 3775.90;
            }
            // Retorna valor
            return valor == antlrResult;
        }

        public static bool TesteVVT(double areaTerreno, double fatorG, double fatorK, double antlrResult)
        {
            var result = Math.Round(Math.Round(fatorG * fatorK, 4) + areaTerreno, 4);
            return result == Math.Round(antlrResult, 4);
        }

        public static bool TesteVVP(double areaTerreno, double antlrResult)
        {
            var result = Math.Round(areaTerreno * 100, 4);
            return result == Math.Round(antlrResult, 4);
        }

        public static bool TesteIPTU(object fisicoEntrada, object resultadosEntrada, double antlrResult)
        {
            var fisico = new
            {
                AreaEdificada = double.Parse((fisicoEntrada as IDictionary<string, object>)["AreaEdificada"].ToString())
            };
            var roteiro = new
            {
                VVT = double.Parse((resultadosEntrada as IDictionary<string, object>)["vvt"].ToString()),
                VVP = double.Parse((resultadosEntrada as IDictionary<string, object>)["vvp"].ToString()),
                ILUMINAO = (resultadosEntrada as IDictionary<string, object>).TryGetValue("ILUMINAO", out object iluminacao) ? iluminacao.ToString().ToNullable<double>() : null,
                LIXO = (resultadosEntrada as IDictionary<string, object>).TryGetValue("LIXO", out object lixo) ? lixo.ToString().ToNullable<double>() : null,
                ESQUINA = (resultadosEntrada as IDictionary<string, object>).TryGetValue("ESQUINA", out object esquina) ? esquina.ToString().ToNullable<double>() : null,
                REFORMADO = (resultadosEntrada as IDictionary<string, object>).TryGetValue("REFORMADO", out object reformado) ? reformado.ToString().ToNullable<double>() : null,
                ESGOTO = (resultadosEntrada as IDictionary<string, object>).TryGetValue("ESGOTO", out object esgoto) ? esgoto.ToString().ToNullable<double>() : null,
            };

            var iptu_base = Math.Round(Math.Round(roteiro.VVT * roteiro.VVP, 4) - fisico.AreaEdificada, 4);

            if (roteiro.ILUMINAO > 0.0 && roteiro.ILUMINAO < 100.0)
            {
                iptu_base = Math.Round(iptu_base + (roteiro.ILUMINAO ?? 0.0), 4);
            }
            else
            {
                iptu_base = Math.Round(Math.Round(iptu_base + (roteiro.ILUMINAO ?? 0.0), 4) + (roteiro.ESGOTO ?? 0.0), 4);
            }

            if (roteiro.LIXO == 100.0 || roteiro.ESQUINA > 50.0)
            {
                iptu_base = Math.Round(iptu_base - (roteiro.REFORMADO ?? 0.0), 4);
            }
            else
            {
                iptu_base = Math.Round(iptu_base + 5000, 4);
            }

            return Math.Round(iptu_base * 0.5, 4) == antlrResult;
        }
    }
}
