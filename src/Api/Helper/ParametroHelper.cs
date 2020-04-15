using Api.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Api.Helper
{
  public static class ParametroHelper
  {


    public static string GetDefaultParametroCodigoSQL(string nome, int exercicio, string codigo) =>
        @$"SELECT Parametros.IdParametro, Parametros.NomeParam, ParametroVlrs.Exercicio, ParametroVlrs.Valor, ParametroVlrs.Codigo
            FROM ParametroVlrs
            INNER JOIN Parametros ON ParametroVlrs.IdParametro = Parametros.IdParametro
            WHERE Parametros.NomeParam = '{nome}' AND ParametroVlrs.Exercicio = '{exercicio}' 
            {(!string.IsNullOrEmpty(codigo) ? $" AND ParametroVlrs.Codigo = {codigo}" : string.Empty)}";


    public static string GetDefaultSQL(Parametro p) => GetDefaultParametroCodigoSQL(p.Nome, p.Exercicio, p.Codigo);

    public static TabelaQuery GetQuery(IEnumerable<Parametro> parametros)
    {
      string consulta = string.Join("\nUNION\n", parametros.Select(x => GetDefaultSQL(x)));

      if (string.IsNullOrEmpty(consulta))
        return null;

      return new TabelaQuery { Tabela = "ParametroVlrs", Consulta = consulta };
    }


  }
}
