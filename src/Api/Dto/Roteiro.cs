

using System.Collections.Generic;

namespace Api.Dto
{
    public class Roteiro{

      public int Id { get; set; }
      public string NomeRoteiro { get; set; }
      public List<Evento> Eventos { get; set; }

    }
}