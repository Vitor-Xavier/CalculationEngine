using Common.Enums;
using Crosscutting.DTO.Formula;
using System.Collections.Generic;

namespace Crosscutting.DTO.Roteiro
{
    public class RoteiroDto
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public SetorOrigem SetorOrigem { get; set; }

        public IEnumerable<FormulaDto> Formulas { get; set; }
    }
}
