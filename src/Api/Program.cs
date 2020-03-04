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
        public static readonly int _maxDegreeOfParallelism = 100;

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
            var caracteristica = TabelaColunaHelper.GetCaracteristica(roteiro.Eventos);

            var dados = await CarregarDados(roteiro.SetorOrigem, tabelas, caracteristica);


            #region Processamento
            Console.WriteLine("\n-- Processamento");

            var stopWatchProcessamento = new Stopwatch();
            stopWatchProcessamento.Start();

            // Principal 
            string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(roteiro.SetorOrigem);
            Parallel.ForEach(dados, new ParallelOptions { MaxDegreeOfParallelism = _maxDegreeOfParallelism }, item =>
            {
                var memory = new Dictionary<string, GenericValueLanguage>();
                foreach (var props in (item.Value as IDictionary<string, object>))
                {
                    if (props.Value is List<object> lista)
                    {
                        int j = 0;
                        lista.ForEach(y =>
                        {
                            foreach (var h in y as IDictionary<string, object>)
                            {
                                memory.Add($"@{props.Key}[{j}].{h.Key}", new GenericValueLanguage(h.Value));
                            };
                            j++;
                        });
                    }
                    else if (props.Value is ExpandoObject obj)
                    {
                        foreach (var x in obj as IDictionary<string, object>)
                        {
                            memory.Add($"@{props.Key}.{x.Key}", new GenericValueLanguage(x.Value));
                        };
                    }
                    else
                        memory.Add($"@{tabelaPrincipal}.{props.Key}", new GenericValueLanguage(props.Value));
                }

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
                        memory.Add($"@Roteiro.{evento.Nome}", new GenericValueLanguage(result));
                    }
                    catch (Exception e)
                    {
                        Exceptions.Value.Enqueue(e);
                    }
                };
                object resultado = memory.Where(m => m.Key.StartsWith("@Roteiro.")).ToDictionary(x => x.Key.Substring(x.Key.LastIndexOf('.')), x => x.Value.Value);
                Resultados.TryAdd(item.Key, resultado);
            });
            stopWatchProcessamento.Stop();
            Console.WriteLine($"Tempo Total Processamento: {stopWatchProcessamento.Elapsed}");
            #endregion

            #region Resultados
            Console.WriteLine($"\n-- Resultados {Resultados.Count}");
            if (Resultados.Count <= 20)
                foreach (var result in Resultados)
                    Console.WriteLine($"{tabelaPrincipal} {result.Key}: {result.Value}");
            #endregion

            #region Erros
            if (Exceptions.Value.Count > 0)
            {
                Console.WriteLine("\n-- Erros");
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

        public static async Task<IDictionary<int, IDictionary<string, object>>> CarregarDados(SetorOrigem setor, IEnumerable<TabelaColuna> tabelas, IEnumerable<CaracteristicaParametros> caracteristica)
        {


            // Diagnóstico
            var stopwatchPre = new Stopwatch();
            stopwatchPre.Start();

            // TODO: BuscaCaracteristica GroupBy

            if (!SetorOrigemHelper.ValidarTabelasSetor(setor, tabelas.Select(g => g.Tabela)))
                throw new Exception($"Erro ao validar tabelas utilizadas para o setor {setor.GetDescription()}");

            var database = new DatabaseConnection();

            var consultas = SetorOrigemHelper.GetQueries(setor, tabelas);

            // Busca por todas as listas de dados requisitadas.
            var keyValuePairs = await database.GetAllData(consultas);

            // Separa as massas de dados em principal, para a tabela principal do setor informado e suas auxiliares.
            string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(setor);
            var principal = keyValuePairs.FirstOrDefault(x => x.Key == tabelaPrincipal)
                .Value.ToDictionary(x => (int)(x as IDictionary<string, object>)["Id"], x => (x as IDictionary<string, object>));
            var auxiliares = keyValuePairs.Where(x => x.Key != tabelaPrincipal).ToList();


            var dados2 = await CarregarDadosCaracteristica(caracteristica, 1);
            var aux2 = dados2.Where(x => x.Key != tabelaPrincipal).ToList();


            // Stop Watch para contar Registro para analise
            stopwatchPre.Stop();
            var totalTabelasAuxiliares = auxiliares.Count();
            var totalRegistroTabelasAuxiliares = auxiliares.Select(x => x.Value.Select(y => y).Count()).Sum();

            var totalCaracteristicas = aux2.Count();
            var totalRegistrosCaracteristicas = aux2.Select(x => x.Value.Select(y => y).Count()).Sum();
            stopwatchPre.Start();

            // Associa as listas de dados a lista da tabela principal.
            foreach (var aux in auxiliares)
            {
                aux.Value.GroupBy(x => (int)(x as IDictionary<string, object>)["IdOrigem"]).ToList().ForEach(x =>
                {
                    if (principal.TryGetValue(x.Key, out IDictionary<string, object> principalValue))
                        principalValue[aux.Key] = x.ToList();
                });
            }

            foreach (var aux in aux2)
            {
                aux.Value.GroupBy(x => (int)(x as IDictionary<string, object>)["IdOrigem"]).ToList().ForEach(x =>
                {
                    if (principal.TryGetValue(x.Key, out IDictionary<string, object> principalValue))
                        principalValue[aux.Key] = x.ToList();
                });
            }

            stopwatchPre.Stop();

            Console.WriteLine($"Quantidade tabelas auxiliares: {totalTabelasAuxiliares}");
            Console.WriteLine($"Quantidade registros tabelas auxiliares: {totalRegistroTabelasAuxiliares}");

            Console.WriteLine($"Quantidade caracteristica: {totalCaracteristicas}");
            Console.WriteLine($"Quantidade registros caracteristica: {totalRegistrosCaracteristicas}");

            Console.WriteLine($"Tempo Total Pré-Processamento: {stopwatchPre.Elapsed}");

            return principal;
        }

        public static async Task<IDictionary<string, IEnumerable<object>>> CarregarDadosCaracteristica(IEnumerable<CaracteristicaParametros> tabelas, int idSelecao)
        {
            // Diagnóstico
            var stopwatchPre = new Stopwatch();
            stopwatchPre.Start();

            var database = new DatabaseConnection();

            // Get Massa de Dados
            string sql = null;

            foreach (var item in tabelas)
                sql += GetDefaultBuscaCaracteristicaSQL(idSelecao, item);

            // Busca por todas as listas de dados requisitadas.
            var keyValuePairs = await database.GetAllDataCaracteristica(tabelas.ToArray(), sql);

            return keyValuePairs;
        }
        private static string GetDefaultBuscaCaracteristicaSQL(int idSelecao, CaracteristicaParametros carac)
        {
            return $@"SELECT IdFisico as IdOrigem, 
                             DescrCaracteristica as DescricaoCaracteristica, 
                             (case when '{carac.ValorFatorCaracteristica ?? "DiferenteTabela"}' = 'Valor' and b.TpCaracteristica = 'Tabela' then c.Valor
		                           when '{carac.ValorFatorCaracteristica ?? "DiferenteTabela"}' = 'Fator' and b.TpCaracteristica = 'Tabela' then c.Fator
                                   when b.TpCaracteristica <> 'Tabela' then a.Vlr end) as Valor
                             from  {carac.TabelaCaracteristica} a
                             inner join RoteiroSelecaoItens selecao on selecao.IdSelecao = {idSelecao} and a.{carac.ColunaCaracteristica} = selecao.IdSelecionado
                             inner join Caracteristicas b on a.IdCaracteristica = b.IdCaracteristica
                             left join CaracteristicaVlrs c on c.IdCaracteristica = b.IdCaracteristica and c.Exercicio = {carac.ExercicioCaracteristica} and a.vlr = c.CodItem
                             WHERE DescrCaracteristica = '{carac.DescricaoCaracteristica}'";

        }
    }
}