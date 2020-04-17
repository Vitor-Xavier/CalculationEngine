using System.Collections.Generic;

namespace Api.Dto
{
    public class AtividadeTabela
    {
        public string Tabela { get; set; }

        public string Descricao { get; set; }

        public string Coluna { get; set; }

        public string Exercicio { get; set; }

        public string ColunaFiltro { get; set; }

        public string ColunaKey { get; set; }

        public List<string> Colunas { get; set; } = new List<string>();
    }
}