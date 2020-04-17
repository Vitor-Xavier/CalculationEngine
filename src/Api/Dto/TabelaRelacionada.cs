using System.Collections.Generic;

namespace Api.Dto
{
    public class TabelaRelacionada
    {
        public string Tabela { get; set; }

        public ICollection<string> Relacionadas { get; set; }
    }
}
