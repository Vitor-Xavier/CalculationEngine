using Api.Database;
using Api.Dto;
using Api.Extensions;
using Api.Helper;
using Api.Services;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class Program
    {
        public static Lazy<ConcurrentQueue<Exception>> Exceptions { get; } = new Lazy<ConcurrentQueue<Exception>>(() => new ConcurrentQueue<Exception>());

        public static ConcurrentDictionary<int, object> Resultados { get; } = new ConcurrentDictionary<int, object>();

        static async Task Main(string[] args)
        {
            Console.WriteLine("-- Roteiro");
            var roteiroService = new RoteiroService();
            var roteiro = await roteiroService.GetRoteiro();
            Console.WriteLine($"Roteiro: {roteiro.Nome}\nQuantidade de Fórmulas: {roteiro.Eventos.Count}");

            // Ferramentas Diagnóstico
            var process = Process.GetCurrentProcess();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("\n-- Pré-Processamento");

            var tabelas = TabelaColunaHelper.GetTabelaColunas(roteiro.Eventos);
            var caracteristicaTabela = TabelaColunaHelper.GetCaracteristicaTabela(roteiro.Eventos);
            var caracteristica = TabelaColunaHelper.GetCaracteristica(roteiro.Eventos);

            var dados = await CarregarDados(roteiro.SetorOrigem, tabelas, caracteristicaTabela, caracteristica);

            #region Processamento
            Console.WriteLine("\n-- Processamento");

            var stopWatchProcessamento = new Stopwatch();
            stopWatchProcessamento.Start();

            // Principal 
            string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(roteiro.SetorOrigem);
            Parallel.ForEach(dados, item =>
            {
                var aux = item.Value.Where(x => x.Value is object[] || x.Value is ExpandoObject);
                var memory = aux.ToDictionary(x => $"@{x.Key}", x => new GenericValueLanguage(x.Value));
                var objPrincipal = item.Value.Except(aux).Aggregate(new ExpandoObject() as IDictionary<string, object>, (a, p) => { a.Add(p.Key, p.Value); return a; });
                memory.Add($"@{tabelaPrincipal}", new GenericValueLanguage(objPrincipal));
                memory.Add("@Roteiro", new GenericValueLanguage(new ExpandoObject()));

                // Execucao das Formulas do Roteiro
                foreach (var evento in roteiro.Eventos)
                {
                    try
                    {
                        // Execução
                        var executeFormula = new ExecuteLanguage();
                        executeFormula.DefaultParserTree(evento.Formula);
                        var result = executeFormula.Execute(memory);

                        // Resultado
                        (memory["@Roteiro"].Value as IDictionary<string, object>).Add(evento.Nome, result.Value);
                    }
                    catch (Exception e)
                    {
                        Exceptions.Value.Enqueue(e);
                    }
                };
                Resultados.TryAdd(item.Key, memory["@Roteiro"].Value);
            });
            stopWatchProcessamento.Stop();
            Console.WriteLine($"Tempo Total Processamento: {stopWatchProcessamento.Elapsed}");
            #endregion

            #region Resultados
            Console.WriteLine($"\n-- Resultados {Resultados.Count}");
            if (Resultados.Count <= 1)
                foreach (var result in Resultados)
                    foreach (var result2 in result.Value as IDictionary<string, object>)
                        Console.WriteLine($"{result2.Key}: {result2.Value}");
            #endregion

            #region Erros
            if (Exceptions.Value.Count > 0)
            {
                Console.WriteLine($"\n-- Erros {Exceptions.Value.Count}");
                foreach (var exception in Exceptions.Value)
                    Console.WriteLine($"Exceção: {exception.Message}");
            }
            #endregion

            // Diagnostic results
            stopwatch.Stop();
            Console.WriteLine("\n-- Geral");
            Console.WriteLine($"Tempo Total: {stopwatch.Elapsed}");
            Console.WriteLine($"Memória máxima utilizada: {(process.PeakWorkingSet64 / 1024f) / 1024f}mb");
        }

        public static async Task<IDictionary<int, IDictionary<string, object>>> CarregarDados(SetorOrigem setor, IEnumerable<TabelaColuna> tabelas, IEnumerable<CaracteristicaTabela> caracteristicaTabela, IEnumerable<Caracteristica> caracteristica)
        {
            int idSelecao = 1;

            // Diagnóstico
            var stopwatchPre = new Stopwatch();
            stopwatchPre.Start();

            var stopwatchPreBD = new Stopwatch();
            
            // TODO: BuscaCaracteristica GroupBy

            if (!SetorOrigemHelper.ValidarTabelasSetor(setor, tabelas.Select(g => g.Tabela)))
                throw new Exception($"Erro ao validar tabelas utilizadas para o setor {setor.GetDescription()}");

            var database = new DatabaseConnection();

            var consultas = SetorOrigemHelper.GetQueries(setor, tabelas, idSelecao);
            var consultasCaracteristicaTabela = CaracteristicaHelper.GetQueries(caracteristicaTabela, idSelecao);

            // TO DO
            //Adicionar no Principal e no GetAllData para trazer os valores
            var consultasCaracteristica = CaracteristicaHelper.GetQueries(caracteristica);

            stopwatchPreBD.Start();
            // Busca por todas as listas de dados requisitadas.
            var keyValuePairs = await database.GetAllData(consultas.Union(consultasCaracteristicaTabela));

            // Separa as massas de dados em principal, para a tabela principal do setor informado e suas auxiliares.
            string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(setor);

            stopwatchPreBD.Stop();

            var principal = keyValuePairs.FirstOrDefault(x => x.Key == tabelaPrincipal)
                .Value.ToDictionary(x => (int)(x as IDictionary<string, object>)["Id"], x => (x as IDictionary<string, object>));

            // Associa as listas de dados a lista da tabela principal.
            foreach (var aux in keyValuePairs.Where(x => x.Key != tabelaPrincipal ))
            {
                foreach (var x in aux.Value.GroupBy(x => (int)(x as IDictionary<string, object>)["IdOrigem"]))
                {
                    if (principal.TryGetValue(x.Key, out IDictionary<string, object> principalValue))
                        principalValue[aux.Key] = x.ToArray();
                };
            }

            

            stopwatchPre.Stop();

            Console.WriteLine($"Tempo Total Pré-Processamento Banco Dados: {stopwatchPreBD.Elapsed}");
            Console.WriteLine($"Tempo Total Pré-Processamento: {stopwatchPre.Elapsed}");

            var auxiliares = keyValuePairs.Where(x => x.Key != tabelaPrincipal);
            var totalTabelasAuxiliares = auxiliares.Count();
            var totalRegistroTabelasAuxiliares = auxiliares.Select(x => x.Value.Select(y => y).Count()).Sum();
            Console.WriteLine($"Quantidade registros principais: {principal.Count}");
            Console.WriteLine($"Quantidade registros auxiliares: {totalRegistroTabelasAuxiliares}");


            return principal;
        }
    }
}