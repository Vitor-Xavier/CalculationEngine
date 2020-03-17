using Api.Dto;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api.Services
{
  public class RoteiroBaseService
  {
    public async Task<Roteiro> GetRoteiro()
    {
      Roteiro roteiro = new Roteiro
      {
        RoteiroId = 1,
        Nome = "Base",
        SetorOrigem = SetorOrigem.Imobiliario,
      };


      Evento basico = new Evento
      {
        Id = 60,
        Nome = "Base",
        Formula = string.Format("lista teste[0] = 10; retorno teste;")
      };

      roteiro.Eventos.Add(basico);

      await Task.Delay(100);
      return roteiro;
    }
  }
}
