using Crosscutting.DTO.Formula;
using Crosscutting.DTO.Variavel;
using System.Collections.Generic;

namespace Crosscutting.DTO.Execucao
{
    public class ExecucaoFormulaDto
    {
        public int SelecaoId { get; set; }

        public FormulaDto Formula { get; set; }

        public IEnumerable<VariavelDto> Variaveis { get; set; } = new HashSet<VariavelDto>();
    }
}
