namespace Api.Dto
{
    public class CaracteristicaParametros
    {
        public string TabelaCaracteristica { get; set; }
        public string DescricaoCaracteristica { get; set; }
        public string IdVinculoCaracteristica { get; set; }
        public string ValorFatorCaracteristica { get; set; }
        public string ExercicioCaracteristica { get; set; }
        public string IdOrigemCaracteristica { get; internal set; }
        public string ColunaCaracteristica { get; internal set; }
        public string ColunaValorCaracteristica { get; internal set; }
    }
}
