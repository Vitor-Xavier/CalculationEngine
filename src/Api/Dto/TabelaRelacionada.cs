using System.Collections.Generic;

namespace Api.Dto
{
    public struct TabelaRelacionada
    {
        public string Tabela { get; set; }

        public ICollection<string> Relacionadas { get; set; }
    }
}
