using System.ComponentModel;

namespace Api.Dto
{
    public enum SetorOrigem
    {
        [Description("Imobiliário")]
        Imobiliario,

        [Description("Contribuinte")]
        Contribuinte,

        [Description("ITBI")]
        ITBI,

        [Description("ITBI Complementar")]
        ITBI_Complementar,

        [Description("Mobiliário")]
        Mobiliario,

        [Description("Parcelamento")]
        Parcelamento,

        [Description("Publicidade")]
        Publicidade,

        [Description("Melhoria")]
        Melhoria,

        [Description("Auto de Infração")]
        AutoInfracao,

        [Description("Projeto")]
        Projeto,

        [Description("Planilha Imobiliário")]
        PlanilhaImobiliario,

        [Description("Ajuizamento")]
        Ajuizamento,

        [Description("Coleta Lixo")]
        ColetaLixo,

        [Description("Taxa Diversas")]
        TaxaDiversa,

        [Description("Alienacão")]
        Alienacao
    }
}
