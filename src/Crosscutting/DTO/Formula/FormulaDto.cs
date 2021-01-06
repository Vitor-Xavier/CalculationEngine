using Common.Enums;

namespace Crosscutting.DTO.Formula
{
    public class FormulaDto
    {
        public string Nome { get; set; }

        public int Ordem { get; set; }

        public string Texto { get; set; }

        public SetorOrigem SetorOrigem { get; set; }
    }
}
