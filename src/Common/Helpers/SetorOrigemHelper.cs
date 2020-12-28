using Common.Enums;
using Common.Exceptions;
using System;

namespace Common.Helpers
{
    public static class SetorOrigemHelper
    {
        public static string[] GetTabelasRelacionadas(SetorOrigem setor) => setor switch
        {
            SetorOrigem.Imobiliario => new string[] { "FacesQuadra" },
            _ => Array.Empty<string>()
        };

        /// <summary>
        /// Busca a tabela principal referente ao Setor Origem informado.
        /// </summary>
        /// <param name="setor">Setor Origem</param>
        /// <returns>Nome da tabela principal</returns>
        public static string GetTabelaPrincipal(SetorOrigem setor) => setor switch
        {
            SetorOrigem.Imobiliario => "Fisicos",
            SetorOrigem.Contribuinte => "Contribuintes",
            SetorOrigem.Global => "Global",
            _ => throw new BadRequestException("Setor não possui uma tabela principal configurada.")
        };

        /// <summary>
        /// Busca as tabelas referentes ao Setor Origem informado.
        /// </summary>
        /// <param name="setor"></param>
        /// <returns></returns>
        public static string[] GetTabelasSetor(SetorOrigem setor) => setor switch
        {
            SetorOrigem.Imobiliario => new string[] { "Fisicos", "FisicoOutros", "FisicoAreas", "FacesQuadra" },
            SetorOrigem.Contribuinte => new string[] { "Contribuintes" },
            SetorOrigem.Global => new string[] { "Caracteristica", "Parametro" },
            _ => throw new BadRequestException("Setor não possui tabelas configuradas.")
        };
    }
}
