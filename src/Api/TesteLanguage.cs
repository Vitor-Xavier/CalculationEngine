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
            } : new { Percentual = (double?) null };
            var fisicoAreas = (entrada as IDictionary<string, object>).ContainsKey("FisicoAreas") ?
            new
            {
                Area = (((entrada as IDictionary<string, object>)["FisicoAreas"] as object[])[0] as IDictionary<string, object>)["Area"]?.ToString().ToNullable<double>(),
            } : new { Area = (double?) null };
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
            var result = Math.Round(areaTerreno * 100.0, 4);
            return result == Math.Round(antlrResult, 4);
        }
    }
}
