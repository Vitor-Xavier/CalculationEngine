using Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Helper
{
    public static class SetorOrigemHelper
    {
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

        public static string GetDefaultSQL(TabelaColuna tabela) =>
            tabela.Tabela.ToUpper() switch
            {
                "FISICO" => $"SELECT IdFisico as Id, {string.Join(", ", tabela.Coluna)} FROM Fisico ORDER BY IdFisico;",
                "FISICOOUTROS" => $"SELECT IdFisicoOutro as Id, IdFisico as IdOrigem, {string.Join(", ", tabela.Coluna)} FROM FisicoOutros ORDER BY IdFisico;",
                "FISICOAREAS" => $"SELECT IdFisicoArea as Id, IdFisico as IdOrigem, {string.Join(", ", tabela.Coluna)} FROM FisicoAreas ORDER BY IdFisico;",
                _ =>
                throw new Exception("Tabela não configurado para o cálculo")
            };
    }
}
