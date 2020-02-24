using System.Collections.Generic;

namespace Api.Dto
{
    public class Evento{

      public int Id { get; set; }
      public string Nome { get; set; }
      public string Formula { get; set; }
      public List<Evento> Eventos { get; set; }
    }
}