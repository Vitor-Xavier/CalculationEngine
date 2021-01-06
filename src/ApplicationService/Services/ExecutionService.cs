using Common.Exceptions;
using Core.DTO;
using Core.Implementation;
using Crosscutting.DTO.Execucao;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationService.Services
{
    public class ExecutionService
    {
        private readonly DynamicSearchService _dynamicSearch;

        public ExecutionService(DynamicSearchService dynamicSearch) => _dynamicSearch = dynamicSearch;

        public async Task<GenericValueLanguage> ExecuteFormula(ExecucaoFormulaDto execucaoFormula, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var execute = new ExecuteLanguage();

            var memory = await _dynamicSearch.SearchDataFormula(execucaoFormula.Formula, execucaoFormula.SelecaoId, cancellationToken);

            foreach (var mem in memory)
                mem.Value.Add("@Variavel", new GenericValueLanguage(execucaoFormula.Variaveis.ToDictionary(v => v.Nome, v => new GenericValueLanguage(v.Valor))));

            if (!memory.Any())
                throw new BadRequestException("Nenhum registro selecionado para execução");

            return execute.Execute(execucaoFormula.Formula.Texto, memory.FirstOrDefault().Value);
        }

        public async Task<IDictionary<int, IDictionary<string, GenericValueLanguage>>> ExecuteRoteiro(ExecucaoRoteiroDto execucaoRoteiro, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var execute = new ExecuteLanguage();

            var formulas = execucaoRoteiro.Roteiro.Formulas.OrderBy(f => f.Ordem).Select(f => new LanguageCode
            {
                Name = f.Nome,
                Code = f.Texto,
                ParseTree = execute.GetParserTree(f.Texto)
            });

            var memory = await _dynamicSearch.SearchDataRoteiro(execucaoRoteiro.Roteiro, execucaoRoteiro.SelecaoId, cancellationToken);

            foreach (var mem in memory)
                mem.Value.Add("@Variavel", new GenericValueLanguage(execucaoRoteiro.Variaveis.ToDictionary(v => v.Nome, v => new GenericValueLanguage(v.Valor))));

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

            return resultados;
        }
    }
}
