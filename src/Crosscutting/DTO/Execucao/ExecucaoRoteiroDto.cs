using Crosscutting.DTO.Roteiro;
using Crosscutting.DTO.Variavel;
using System.Collections.Generic;

namespace Crosscutting.DTO.Execucao
{
    public class ExecucaoRoteiroDto
    {
        public int SelecaoId { get; set; }

        public RoteiroDto Roteiro { get; set; }

        public IEnumerable<VariavelDto> Variaveis { get; set; } = new HashSet<VariavelDto>();
    }
}
