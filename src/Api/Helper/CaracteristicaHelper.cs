using Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Helper
{
    public static class CaracteristicaHelper
    {

        public static IEnumerable<TabelaQuery> GetQueries(IEnumerable<CaracteristicaTabela> caracteristicaTabela, int idSelecao)
        {
            foreach (var caracteristica in caracteristicaTabela)
            {
                string sql = string.Format(CaracteristicaHelper.GetDefaultBuscaCaracteristicaSQL(idSelecao, caracteristica));
                string tabela = string.Format("{0}.{1}", caracteristica.Tabela, caracteristica.Descricao);

                yield return new TabelaQuery { Tabela = tabela, Consulta = sql };
            }
        }
    
        public static IEnumerable<TabelaQuery> GetQueries(IEnumerable<Caracteristica> caracteristicas)
        {
            foreach (var caracteristica in caracteristicas)
            {
                string sql = string.Format(CaracteristicaHelper.GetDefaultCaracteristicaSQL(caracteristica));
                string caracteristicaTabela = string.Format("{0}.{1}", "Caracteristica", caracteristica.Descricao);

                yield return new TabelaQuery { Tabela = caracteristicaTabela, Consulta = sql };
            }
        }

        public static string GetDefaultBuscaCaracteristicaSQL(int idSelecao, CaracteristicaTabela dto)
        {
            return $@"SELECT a.{dto.Coluna} as IdOrigem, 
                                DescrCaracteristica as DescricaoCaracteristica, 
                                (case when '{dto.ValorFator ?? "DiferenteTabela"}' = 'Valor' and b.TpCaracteristica = 'Tabela' then c.Valor
                                    when '{dto.ValorFator ?? "DiferenteTabela"}' = 'Fator' and b.TpCaracteristica = 'Tabela' then c.Fator
                                    when b.TpCaracteristica <> 'Tabela' then a.Vlr end) as Valor
                                from  {dto.Tabela} a
                                inner join RoteiroSelecaoItens selecao on selecao.IdSelecao = {idSelecao} and a.{dto.Coluna} = selecao.IdSelecionado
                                inner join Caracteristicas b on a.IdCaracteristica = b.IdCaracteristica
                                left join CaracteristicaVlrs c on c.IdCaracteristica = b.IdCaracteristica and c.Exercicio = {dto.Exercicio} and a.vlr = c.CodItem
                                WHERE DescrCaracteristica = '{dto.Descricao}'";

        }

        public static string GetDefaultCaracteristicaSQL(Caracteristica dto)
        {
            return $@"SELECT TOP 1 
                        DescrCaracteristica as DescricaoCaracteristica, 
                        (case when '{dto.ValorFator}' = 'Valor' then c.Valor
                            when '{dto.ValorFator}' = 'Fator' then c.Fator
                            else null end) as Valor
                        from  Caracteristicas b
                        inner join CaracteristicaVlrs c on c.IdCaracteristica = b.IdCaracteristica and c.Exercicio = {dto.Exercicio} 
                        WHERE DescrCaracteristica = '{dto.Descricao}' and c.CodItem = '{dto.Codigo}'  ORDER BY b.IdCaracteristica DESC ";

        }

    }
}
