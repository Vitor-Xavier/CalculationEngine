using Common.Exceptions;
using Core.DTO;
using Core.Implementation;
using Crosscutting.DTO.Execucao;
using Crosscutting.DTO.Resultado;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationService.Services
{
    public class ExecutionService
    {
        private readonly ILogger<ExecutionService> _logger;

        private readonly DynamicSearchService _dynamicSearch;

        public ExecutionService(ILogger<ExecutionService> logger, 
                                DynamicSearchService dynamicSearch)
        {
            _logger = logger;
            _dynamicSearch = dynamicSearch;
        }

        public async Task<ResultadoFormulaDto> ExecuteFormula(ExecucaoFormulaDto execucaoFormula, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cpuStart = Process.GetCurrentProcess().TotalProcessorTime;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var execute = new ExecuteLanguage();

            var parseTree = execute.GetParserTree(execucaoFormula.Formula.Texto);

            if (execute.LanguageErrorListener.SyntaxErrors.Any())
                throw new BadRequestException(string.Join("\n", execute.LanguageErrorListener.SyntaxErrors));

            var memory = await _dynamicSearch.SearchDataFormula(execucaoFormula.Formula, execucaoFormula.SelecaoId, cancellationToken);

            if (execucaoFormula.Variaveis.Any())
            {
                var variaveis = new GenericValueLanguage(execucaoFormula.Variaveis.ToDictionary(v => v.Nome, v => new GenericValueLanguage(v.Valor)));
                foreach (var m in memory)
                    m.Value.Add("@Variavel", variaveis);
            }

            if (!memory.Any())
                throw new BadRequestException("Nenhum registro selecionado para execução");

            var resultado = execute.ExecuteParseTree(parseTree, memory.FirstOrDefault().Value);

            stopwatch.Stop();
            var cpuEnd = Process.GetCurrentProcess().TotalProcessorTime;

            _logger.LogInformation($"Tempo total: {stopwatch.Elapsed}");
            _logger.LogInformation($"Uso médio de processamento: {Math.Round((cpuEnd - cpuStart).TotalMilliseconds / (Environment.ProcessorCount * stopwatch.ElapsedMilliseconds)):P}");

            return new ResultadoFormulaDto
            {
                NomeFormula = execucaoFormula.Formula.Nome,
                Valor = resultado.Value
            };
        }

        public async Task<ResultadoDto> ExecuteRoteiro(ExecucaoRoteiroDto execucaoRoteiro, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cpuStart = Process.GetCurrentProcess().TotalProcessorTime;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var execute = new ExecuteLanguage();

            var formulas = execucaoRoteiro.Roteiro.Formulas.OrderBy(f => f.Ordem).Select(f => new LanguageCode
            {
                Name = f.Nome,
                Code = f.Texto,
                ParseTree = execute.GetParserTree(f.Texto)
            });

            if (execute.LanguageErrorListener.SyntaxErrors.Any())
                throw new BadRequestException(string.Join("\n", execute.LanguageErrorListener.SyntaxErrors));

            var memory = await _dynamicSearch.SearchDataRoteiro(execucaoRoteiro.Roteiro, execucaoRoteiro.SelecaoId, cancellationToken);

            if (execucaoRoteiro.Variaveis.Any())
            {
                var variaveis = new GenericValueLanguage(execucaoRoteiro.Variaveis.ToDictionary(v => v.Nome, v => new GenericValueLanguage(v.Valor)));
                foreach (var m in memory)
                    m.Value.Add("@Variavel", variaveis);
            }

            if (!memory.Any())
                throw new BadRequestException("Nenhum registro selecionado para execução");

            var resultados = new ConcurrentDictionary<int, IDictionary<string, GenericValueLanguage>>();

            var parallelOptions = new ParallelOptions { CancellationToken = cancellationToken };
            Parallel.ForEach(memory, item =>
            {
                item.Value.Add("@Roteiro", new GenericValueLanguage(new Dictionary<string, GenericValueLanguage>()));

                foreach (var formula in formulas)
                {
                    try
                    {
                        var result = execute.ExecuteParseTree(formula.ParseTree, item.Value);

                        (item.Value["@Roteiro"].Value as IDictionary<string, GenericValueLanguage>).Add(formula.Name, result);
                    }
                    catch (LanguageException e)
                    {
                        throw new BadRequestException(e.LanguageError.Message, e);
                    }
                }

                resultados.TryAdd(item.Key, item.Value["@Roteiro"].Value as IDictionary<string, GenericValueLanguage>);
            });

            stopwatch.Stop();
            var cpuEnd = Process.GetCurrentProcess().TotalProcessorTime;

            _logger.LogInformation($"Tempo total: {stopwatch.Elapsed}");
            _logger.LogInformation($"Uso médio de processamento: {Math.Round((cpuEnd - cpuStart).TotalMilliseconds / (Environment.ProcessorCount * stopwatch.ElapsedMilliseconds)):P}");

            return new ResultadoDto
            {
                NomeRoteiro = execucaoRoteiro.Roteiro.Nome,
                Valores = resultados.ToDictionary(r => r.Key, r => r.Value.ToDictionary(v => v.Key, v => v.Value.Value))
            };
        }
    }
}
