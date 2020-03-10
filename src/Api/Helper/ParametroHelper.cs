using Api.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Api.Helper
{
    public static class ParametroHelper
    {
        public static string GetDefaultParametroUnicoSQL(string nome, int? exercicio) =>
            @$"SELECT TOP 1 Parametros.IdParametro, Parametros.NomeParam, ParametroVlrs.Exercicio, ParametroVlrs.Valor, ParametroVlrs.Codigo
            FROM ParametroVlrs
            INNER JOIN Parametros ON ParametroVlrs.IdParametro = Parametros.IdParametro
            WHERE Parametros.NomeParam = '{nome}'
            {(exercicio.HasValue ? $" AND ParametroVlrs.Exercicio = {exercicio}" : string.Empty)}";

        public static string GetDefaultParametroCodigoSQL(string nome, string codigo, int? exercicio) =>
            @$"SELECT TOP 1 Parametros.IdParametro, Parametros.NomeParam, ParametroVlrs.Exercicio, ParametroVlrs.Valor, ParametroVlrs.Codigo
            FROM ParametroVlrs
            INNER JOIN Parametros ON ParametroVlrs.IdParametro = Parametros.IdParametro
            WHERE Parametros.NomeParam = '{nome}' AND ParametroVlrs.Codigo = '{codigo}' 
            {(exercicio.HasValue ? $" AND ParametroVlrs.Exercicio = {exercicio}" : string.Empty)}";
        
        public static string GetDefaultParametroIntervaloSQL(string nome, string valor, int? exercicio) =>
            @$"SELECT TOP 1 Parametros.IdParametro, Parametros.NomeParam, ParametroVlrs.Exercicio, ParametroVlrs.Valor, {valor} as Codigo
            FROM ParametroVlrs
            INNER JOIN Parametros ON ParametroVlrs.IdParametro = Parametros.IdParametro
            WHERE Parametros.NomeParam = '{nome}' AND {valor} BETWEEN FaixaInicial AND FaixaFinal 
            {(exercicio.HasValue ? $" AND ParametroVlrs.Exercicio = {exercicio}" : string.Empty)}";

        public static string GetDefaultSQL(Parametro parametro) => parametro switch
        {
            var p when !string.IsNullOrEmpty(p.Valor) => GetDefaultParametroIntervaloSQL(p.Nome, p.Valor, p.Exercicio),
            var p when !string.IsNullOrEmpty(p.Codigo) => GetDefaultParametroCodigoSQL(p.Nome, p.Codigo, p.Exercicio),
            _ => GetDefaultParametroUnicoSQL(parametro.Nome, parametro.Exercicio)
        };

        public static TabelaQuery GetQuery(IEnumerable<Parametro> parametros) =>
            new TabelaQuery { Tabela = "ParametroVlrs", Consulta = string.Join("\nUNION\n", parametros.Select(x => GetDefaultSQL(x))) };
    }
}
