using Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Helper
{
    public static class SetorOrigemHelper
    {
        public static readonly IEnumerable<TabelaRelacionada> TabelaRelacionadas = new List<TabelaRelacionada>
        {
            { new TabelaRelacionada { Tabela = "Fisico", Relacionadas = new string[] { "FacesdaQuadra" } } }
        };

        public static string GetTabelaPrincipal(SetorOrigem setor) =>
            setor switch
            {
                SetorOrigem.Imobiliario => "Fisico",
                SetorOrigem.Contribuinte => "Contribuintes",
                SetorOrigem.ITBI => "Itbi",
                SetorOrigem.ITBI_Complementar => "ItbiComplementar",
                SetorOrigem.Mobiliario => "Ccm",
                SetorOrigem.Parcelamento => "Parcelamentos",
                _ => throw new Exception("Setor não possui uma tabela principal configurada.")
            };

        public static IEnumerable<string> GetTabelasSetor(SetorOrigem setor) =>
            setor switch
            {
                SetorOrigem.Imobiliario => new string[] { "Fisico", "FisicoOutros", "FisicoAreas", "FacesdaQuadra" },
                SetorOrigem.Contribuinte => new string[] { "Contribuintes" },
                SetorOrigem.ITBI => new string[] { "Itbi", "ItbiComplementar", "ItbiFisicos", "ItbiOutros" },
                SetorOrigem.ITBI_Complementar => new string[] { "Itbi", "ItbiComplementar", "ItbiFisicos", "ItbiOutros" },
                SetorOrigem.Mobiliario => new string[] { "Ccm", "CcmAtividades", "CcmCnae" },
                SetorOrigem.Parcelamento => new string[] { "Parcelamentos" },
                _ => throw new Exception("Setor não possui tabelas configuradas.")
            };

        public static bool ValidarTabelasSetor(SetorOrigem setor, IEnumerable<string> tabelas) =>
            tabelas.All(GetTabelasSetor(setor).Contains);

        public static string GetDefaultSQL(string tabela, int selecaoId) =>
            tabela.ToUpper() switch
            {
                "FISICO" => "SELECT IdFisico as [Fisico.Id], {0} FROM Fisico " +
                           $"INNER JOIN RoteiroSelecaoItens selecao ON selecao.IdSelecao = {selecaoId} AND IdFisico = selecao.IdSelecionado",
                "FACESDAQUADRA" => " LEFT JOIN FacesdaQuadra ON Fisico.IdFacedaQuadra = FacesdaQuadra.IdFacedaQuadra",
                "FISICOOUTROS" => "SELECT IdFisicoOutro as [FisicoOutros.Id], IdFisico as [FisicoOutros.IdOrigem], {0} FROM FisicoOutros " +
                           $"INNER JOIN RoteiroSelecaoItens selecao ON selecao.IdSelecao = {selecaoId} AND IdFisico = selecao.IdSelecionado ORDER BY IdFisico",
                "FISICOAREAS" => "SELECT IdFisicoArea as [FisicoAreas.Id], IdFisico as [FisicoAreas.IdOrigem], {0} FROM FisicoAreas " +
                           $"INNER JOIN RoteiroSelecaoItens selecao ON selecao.IdSelecao = {selecaoId} AND IdFisico = selecao.IdSelecionado ORDER BY IdFisico",
                _ =>
                throw new Exception("Tabela não configurada para o cálculo")
            };

        public static IEnumerable<TabelaQuery> GetQueries(SetorOrigem setor, IEnumerable<TabelaColuna> tabelas, int selecaoId)
        {
            string tabelaPrincipal = GetTabelaPrincipal(setor);
            var principal = tabelas.FirstOrDefault(x => x.Tabela == GetTabelaPrincipal(setor));

            var relacionados = tabelas.Where(x => TabelaRelacionadas.First(y => y.Tabela == tabelaPrincipal).Relacionadas.Contains(x.Tabela));
            principal.Coluna = principal.Coluna.Union(relacionados.SelectMany(x => x.Coluna.Select(y => $"{x.Tabela}.{y} as [{x.Tabela}.{y}]"))).ToList();
            string sqlRelacionados = string.Join("\\s", relacionados.Select(x => GetDefaultSQL(x.Tabela, selecaoId)));

            foreach (var tabela in tabelas.Except(relacionados))
            {
                string sql = string.Format(GetDefaultSQL(tabela.Tabela, selecaoId), string.Join(",", tabela.Coluna));
                if (tabela.Tabela == tabelaPrincipal)
                    sql += sqlRelacionados + $" ORDER BY [{tabela.Tabela}.Id];";
                yield return new TabelaQuery { Tabela = tabela.Tabela, Consulta = sql };
            }
        }
    }
}
