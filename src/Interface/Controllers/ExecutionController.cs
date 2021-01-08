using ApplicationService.Services;
using Crosscutting.DTO.Execucao;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Interface.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionController : ControllerBase
    {
        private readonly ExecutionService _service;

        public ExecutionController(ExecutionService service) => _service = service;

        /// <summary>
        /// Execução de Fórmula de Cálculo.
        /// </summary>
        /// <param name="dto">Dados da Execução</param>
        /// <returns>Resultado da Execução</returns>
        [HttpPost("Formula")]
        public async Task<IActionResult> PostFormula(ExecucaoFormulaDto dto)
        {
            var result = await _service.ExecuteFormula(dto);
            return Ok(result);
        }

        /// <summary>
        /// Execução de Roteiro de Cálculo.
        /// </summary>
        /// <param name="dto">Dados da Execução</param>
        /// <returns>Lista de Resultados da Execução</returns>
        [HttpPost("Roteiro")]
        public async Task<IActionResult> PostRoteiro(ExecucaoRoteiroDto dto)
        {
            var result = await _service.ExecuteRoteiro(dto);
            return Ok(result);
        }
    }
}
