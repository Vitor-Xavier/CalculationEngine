using System;
using System.Collections.Generic;

namespace Crosscutting.DTO.Resultado
{
    public class ResultadoDto
    {
        public string NomeRoteiro { get; set; }

        public Dictionary<int, Dictionary<string, object>> Valores { get; set; }
    }
}
