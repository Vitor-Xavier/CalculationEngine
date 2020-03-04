using Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Helper
{
    public static class BuscaCaracteristicaHelper
    {

        public static IEnumerable<TabelaQuery> GetQueries(IEnumerable<CaracteristicaParametros> tabelas, int idSelecao)
        {

            foreach (var caracteristica in tabelas)
            {

                string sql = string.Format(BuscaCaracteristicaHelper.GetDefaultBuscaCaracteristicaSQL(idSelecao, caracteristica));
                string caracteristicaTabela = string.Format("{0}.{1}", caracteristica.Tabela, caracteristica.Descricao);

                yield return new TabelaQuery { Tabela = caracteristicaTabela, Consulta = sql };
            }
        }
    


        public static string GetDefaultBuscaCaracteristicaSQL(int idSelecao, CaracteristicaParametros carac)
        {
            return $@"SELECT IdFisico as IdOrigem, 
                                DescrCaracteristica as DescricaoCaracteristica, 
                                (case when '{carac.ValorFator ?? "DiferenteTabela"}' = 'Valor' and b.TpCaracteristica = 'Tabela' then c.Valor
                                    when '{carac.ValorFator ?? "DiferenteTabela"}' = 'Fator' and b.TpCaracteristica = 'Tabela' then c.Fator
                                    when b.TpCaracteristica <> 'Tabela' then a.Vlr end) as Valor
                                from  {carac.Tabela} a
                                inner join RoteiroSelecaoItens selecao on selecao.IdSelecao = {idSelecao} and a.{carac.Coluna} = selecao.IdSelecionado
                                inner join Caracteristicas b on a.IdCaracteristica = b.IdCaracteristica
                                left join CaracteristicaVlrs c on c.IdCaracteristica = b.IdCaracteristica and c.Exercicio = {carac.Exercicio} and a.vlr = c.CodItem
                                WHERE DescrCaracteristica = '{carac.Descricao}'";

        }
    }
}
