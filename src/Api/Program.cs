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
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class Program
    {
        public static readonly int _maxDegreeOfParallelism = 100;

        public static SetorOrigem SetorOrigem { get; set; }

        public static Lazy<ConcurrentQueue<Exception>> Exceptions { get; } = new Lazy<ConcurrentQueue<Exception>>(() => new ConcurrentQueue<Exception>());

        public static ConcurrentDictionary<int, object> Resultados { get; } = new ConcurrentDictionary<int, object>();

        static async Task Main(string[] args)
        {
            SetorOrigem = SetorOrigem.Imobiliario;

            Console.WriteLine("-- Roteiro");
            var roteiroService = new RoteiroService();
            var roteiro = await roteiroService.GetRoteiro();
            Console.WriteLine($"Roteiro: {roteiro.NomeRoteiro}\nQuantidade de Fórmulas: {roteiro.Eventos.Count}");

            // Ferramentas Diagnóstico
            var process = Process.GetCurrentProcess();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("\n-- Pré-Processamento");
            var tabelas = TabelaColunaHelper.GetTabelaColunas(roteiro.Eventos);
            var dados = await CarregarDados(SetorOrigem, tabelas);

            #region Processamento
            Console.WriteLine("\n-- Processamento");

            var stopWatchProcessamento = new Stopwatch();
            stopWatchProcessamento.Start();

            // Principal 
            string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(SetorOrigem);
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
                            (y as IDictionary<string, object>).ToList().ForEach(h =>
                            {
                                memory.Add($"@{props.Key}[{j}].{h.Key}", new GenericValueLanguage(h.Value));
                            });
                            j++;
                        });
                    }
                    else
                        memory.Add($"@{tabelaPrincipal}.{props.Key}", new GenericValueLanguage(props.Value));
                }

                // Execucao das Formulas do Roteiro
                roteiro.Eventos.ForEach(evento =>
                {
                    try
                    {
                        // Execução
                        var executeFormula = new ExecuteLanguage();
                        executeFormula.DefaultParserTree(evento.Formula);
                        var value = executeFormula.Execute(memory);

                        // Resultado
                        memory.Add($"@Roteiro.{evento.Nome}", new GenericValueLanguage(value));
                    }
                    catch (Exception e)
                    {
                        Exceptions.Value.Enqueue(e);
                    }
                });
                object resultado = memory.Where(m => m.Key.StartsWith("@Roteiro.")).ToDictionary(x => x.Key.Replace("@Roteiro.", string.Empty), x => x.Value.Value);
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

        public static async Task<IDictionary<int, IDictionary<string, object>>> CarregarDados(SetorOrigem setor, IEnumerable<TabelaColuna> tabelas)
        {
            // Diagnóstico
            var stopwatchPre = new Stopwatch();
            stopwatchPre.Start();

            // TODO: BuscaCaracteristica GroupBy

            if (!SetorOrigemHelper.ValidarTabelasSetor(setor, tabelas.Select(g => g.Tabela)))
                throw new Exception($"Erro ao validar tabelas utilizadas para o setor {setor.GetDescription()}");

            var database = new DatabaseConnection();

            // Get Massa de Dados
            string sql = null;
            foreach (var item in tabelas)
                sql += SetorOrigemHelper.GetDefaultSQL(item);

            // Busca por todas as listas de dados requisitadas.
            var keyValuePairs = await database.GetAllData(tabelas.Select(x => x.Tabela).ToArray(), sql);

            // Separa as massas de dados em principal, para a tabela principal do setor informado e suas auxiliares.
            string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(setor);
            var principal = keyValuePairs.FirstOrDefault(x => x.Key == tabelaPrincipal)
                .Value.ToDictionary(x => (int)(x as IDictionary<string, object>)["Id"], x => (x as IDictionary<string, object>));
            var auxiliares = keyValuePairs.Where(x => x.Key != tabelaPrincipal);

            // Associa as listas de dados a lista da tabela principal.
            foreach (var aux in auxiliares)
            {
                aux.Value.GroupBy(x => (int)(x as IDictionary<string, object>)["IdOrigem"]).ToList().ForEach(x =>
                {
                    if (principal.TryGetValue(x.Key, out IDictionary<string, object> principalValue))
                        principalValue[aux.Key] = x.ToList();
                });
            }

            stopwatchPre.Stop();
            Console.WriteLine($"Tempo Total Pré-Processamento: {stopwatchPre.Elapsed}");

            return principal;
        }

        private static string GetDefaultBuscaCaracteristicaSQL(string ids, CaracteristicaParametros carac)
        {

            return $@"SELECT IdFisico as IdOrigem, 
                             DescrCaracteristica as DescricaoCaracteristica, 
                             (case when '{carac.ValorFatorCaracteristica ?? "DiferenteTabela"}' = 'Valor' and b.TpCaracteristica = 'Tabela' then c.Valor
		                           when '{carac.ValorFatorCaracteristica ?? "DiferenteTabela"}' = 'Fator' and b.TpCaracteristica = 'Tabela' then c.Fator
                                   when b.TpCaracteristica <> 'Tabela' then a.Vlr end) as Valor
                             from  {carac.TabelaCaracteristica} a
                             inner join Caracteristicas b on a.IdCaracteristica = b.IdCaracteristica
                             inner join CaracteristicaVlrs c on c.IdCaracteristica = b.IdCaracteristica and c.Exercicio = {carac.ExercicioCaracteristica} and a.vlr = c.CodItem
                             WHERE DescrCaracteristica = '{carac.DescricaoCaracteristica}' and {carac.ColunaCaracteristica} in ({ids})";

        }
    }
}