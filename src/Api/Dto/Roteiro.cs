using System.Collections.Generic;

namespace Api.Dto
{
    public class Roteiro
    {
        public int RoteiroId { get; set; }
        public string Nome { get; set; }
        public SetorOrigem SetorOrigem { get; set; }
        public virtual ICollection<Evento> Eventos { get; } = new HashSet<Evento>();
    }
}