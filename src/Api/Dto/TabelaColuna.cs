using System.Collections.Generic;

namespace Api.Dto
{
    public class TabelaColuna
    {
        public string Tabela { get; set; }

        public virtual ICollection<string> Coluna { get; set; } = new HashSet<string>();
    }
}