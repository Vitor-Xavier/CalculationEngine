using Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Helper
{
  public static class AtividadeHelper
  {
    public static IEnumerable<TabelaQuery> GetQueries(int idSelecao, IEnumerable<AtividadeTabela> atividades, List<IDictionary<string, object>> keysT)
    {
      foreach (var atividade in atividades)
      {
        string tabelaKey = string.Empty;
        foreach (IDictionary<string, object> item in keysT)
        {
          item.TryGetValue("TABLENAME", out object obj1);
          if (obj1 != null && obj1.ToString().ToUpper() == atividade.Tabela.ToUpper())
          {
            item.TryGetValue("PRIMARYKEYCOLUMN", out object obj2);
            tabelaKey = "tabela." + obj2.ToString();
            break;
          }

        }

        atividade.ColunaKey = tabelaKey;

        string sql = string.Format(AtividadeHelper.GetDefaultAtividadeSQL(idSelecao, atividade));
        string caracteristicaTabela = string.Format("{0}.{1}.{2}", "Atividade", atividade.Descricao, atividade.Exercicio);

        yield return new TabelaQuery { Tabela = caracteristicaTabela, Consulta = sql };
      }
    }

    public static string GetDefaultAtividadeSQL(int idSelecao, AtividadeTabela atividade)
    {
      idSelecao = 5427;
      var allColunas = atividade.Colunas.Any() ? string.Join(',', atividade.Colunas) : "Atividades.*, AtividadesVlrs.*";
      return $@"select 797 as IdOrigem,{atividade.ColunaKey}, { allColunas }
                        from {atividade.Tabela} as tabela
                        inner join Atividades on Atividades.IdAtividade = tabela.{atividade.ColunaFiltro}
                        left join AtividadesVlrs on AtividadesVlrs.IdAtividade = Atividades.IdAtividade
                        where tabela.{atividade.Coluna} = {idSelecao} and Atividades.tpAtividade = '{atividade.Descricao}'
                        order by tabela.{atividade.Coluna} desc";

    }

  }
}
