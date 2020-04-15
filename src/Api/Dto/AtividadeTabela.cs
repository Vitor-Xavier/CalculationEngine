using System.Collections.Generic;

namespace Api.Dto {
    public class AtividadeTabela {

        public AtividadeTabela()
        {
            Colunas = new List<string>();

        }

        public string Tabela { get; set; }
        public string Descricao { get; set; }
        public string Coluna { get; set; }
        public string Exercicio { get; set; }
        public List<string> Colunas { get; set; }
    public string ColunaFiltro { get; internal set; }
    public string ColunaKey { get; internal set; }
  }
}