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
            Console.WriteLine("# Cálculo Tributário");
            Console.WriteLine("\n## Roteiro\n");
            var roteiroService = new RoteiroBaseService();
            var roteiro = await roteiroService.GetRoteiro();
            Console.WriteLine("| Roteiro  | Valor      |");
            Console.WriteLine("|----------|------------|");
            Console.WriteLine($"| Nome     | {roteiro.Nome,-10} |");
            Console.WriteLine($"| Fórmulas | {roteiro.Eventos.Count,-10} |");

            // Ferramentas Diagnóstico
            var stopwatch = new Stopwatch();
            var startProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
            stopwatch.Start();

            Console.WriteLine("\n## Pré-Processamento\n");

            var tabelas = TabelaColunaHelper.GetTabelaColunas(roteiro);
            var caracteristicaTabela = TabelaColunaHelper.GetCaracteristicaTabela(roteiro.Eventos);
            var caracteristica = TabelaColunaHelper.GetCaracteristica(roteiro.Eventos);
            var parametros = TabelaColunaHelper.GetParametros(roteiro.Eventos);

            var dados = await CarregarDados(roteiro.SetorOrigem, tabelas, caracteristicaTabela, caracteristica);
            //var dadosGlobais = await CarregarDadosGlobal(roteiro.SetorOrigem, caracteristica, parametros);

            //var memoryGlobal = dadosGlobais.ToDictionary(x => $"@{x.Key}", x => new GenericValueLanguage(x.Key == "Caracteristica" ? x.Value.FirstOrDefault() : x.Value.ToArray()));

            #region Processamento
            Console.WriteLine("\n## Processamento\n");

            var stopWatchProcessamento = new Stopwatch();
            var cpuProcessamentoStart = Process.GetCurrentProcess().TotalProcessorTime;
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
            var cpuProcessamentoEnd = Process.GetCurrentProcess().TotalProcessorTime;
            double cpuTotalProcessamento = (cpuProcessamentoEnd - cpuProcessamentoStart).TotalMilliseconds;

            Console.WriteLine("| Medição        | Utilização       |");
            Console.WriteLine("|----------------|------------------|");
            Console.WriteLine($"| Tempo Total    | {stopWatchProcessamento.Elapsed} |");
            Console.WriteLine($"| CPU média      | {Math.Round((cpuTotalProcessamento / (Environment.ProcessorCount * stopWatchProcessamento.ElapsedMilliseconds)) * 100) + "%",-16} |\n");
            #endregion

            #region Resultados
            Console.WriteLine($"\n## Resultados\n");
            Console.WriteLine("| Resultados                | Valor           |");
            Console.WriteLine("|---------------------------|-----------------|");
            if (Resultados.Count <= 1)
                foreach (var result in Resultados)
                    foreach (var result2 in result.Value as IDictionary<string, object>)
                        Console.WriteLine($"| {result2.Key,-25} | {result2.Value,-15} |");
            Console.WriteLine($"| {"Total",-25} | {Resultados.Count,-15} |");
            #endregion

            #region Erros
            if (Exceptions.Value.Count > 0)
            {
                Console.WriteLine($"\n## Erros\n");
                Console.WriteLine($"| {"Erros",-20} | {"Valor",-15} |");
                Console.WriteLine($"|----------------------|-----------------|");
                foreach (var exception in Exceptions.Value)
                    Console.WriteLine($"Exceção: {exception.Message}");
                Console.WriteLine($"| {"Total",-20} | {Exceptions.Value.Count,-15} |");
            }
            #endregion

            // Diagnostic results
            stopwatch.Stop();
            var endProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
            double totalProcessorTime = (endProcessorTime - startProcessorTime).TotalMilliseconds;

            //TestarFormulas(dados);

            Console.WriteLine("\n## Geral\n");
            Console.WriteLine("| Medição        | Utilização       |");
            Console.WriteLine("|----------------|------------------|");
            Console.WriteLine($"| Tempo Total    | {stopwatch.Elapsed} |");
            Console.WriteLine($"| Memória máxima | {(Process.GetCurrentProcess().PeakWorkingSet64 / 1024f) / 1024f + "mb",-16} |");
            Console.WriteLine($"| CPU média      | {Math.Round((totalProcessorTime / (Environment.ProcessorCount * stopwatch.ElapsedMilliseconds)) * 100) + "%",-16:d2} |\n");
        }

        public static async Task<IDictionary<string, IEnumerable<object>>> CarregarDadosGlobal(SetorOrigem setor, IEnumerable<Caracteristica> caracteristica, IEnumerable<Parametro> parametros)
        {

            // Diagnóstico
            var stopwatchPre = new Stopwatch();
            stopwatchPre.Start();

            var stopwatchPreBD = new Stopwatch();

            // TODO: BuscaCaracteristica GroupBy

            var database = new DatabaseConnection();

            // TO DO
            //Adicionar no Principal e no GetAllData para trazer os valores
            var consultasCaracteristica = CaracteristicaHelper.GetQueries(caracteristica);
            //var consultasParametro = ParametroHelper.GetQueries(parametros);
            var consultaParametro = ParametroHelper.GetQuery(parametros);

            stopwatchPreBD.Start();

            // Busca por todas as listas de dados requisitadas.
            var keyValuePairsGlobal = await database.GetAllData(consultasCaracteristica.Union(new TabelaQuery[] { consultaParametro }));

            // Separa as massas de dados em principal, para a tabela principal do setor informado e suas auxiliares.

            var tabelaGlobal = SetorOrigemHelper.GetTabelasSetor(SetorOrigem.Global);

            stopwatchPreBD.Stop();

            stopwatchPre.Stop();

            Console.WriteLine("### Dados Globais\n");
            Console.WriteLine("| Medição              | Utilização       |");
            Console.WriteLine("|----------------------|------------------|");
            Console.WriteLine($"| Registros globais    | {keyValuePairsGlobal.Count,-16} |");
            Console.WriteLine($"| Tempo Banco de Dados | {stopwatchPreBD.Elapsed} |");
            Console.WriteLine($"| Tempo Total          | {stopwatchPre.Elapsed} |");

            return keyValuePairsGlobal;
        }

        public static async Task<IDictionary<int, IDictionary<string, object>>> CarregarDados(SetorOrigem setor, IEnumerable<TabelaColuna> tabelas, IEnumerable<CaracteristicaTabela> caracteristicaTabela, IEnumerable<Caracteristica> caracteristica)
        {
            int idSelecao = 3;

            // Diagnóstico
            var stopwatchPre = new Stopwatch();
            stopwatchPre.Start();

            var stopwatchPreBD = new Stopwatch();

            // TODO: BuscaCaracteristica GroupBy

            if (!SetorOrigemHelper.ValidarTabelasSetor(setor, tabelas.Select(g => g.Tabela)))
                throw new Exception($"Erro ao validar tabelas utilizadas para o setor {setor.GetDescription()}");

            var database = new DatabaseConnection();

            IEnumerable<TabelaQuery> consultas = Enumerable.Empty<TabelaQuery>();
            if(tabelas.Any() && tabelas.Count() > 0)    
                consultas = SetorOrigemHelper.GetQueries(setor, tabelas, idSelecao);

            IEnumerable<TabelaQuery> consultasCaracteristicaTabela = Enumerable.Empty<TabelaQuery>();
            if(caracteristicaTabela.Any() && tabelas.Count() > 0)    
                consultasCaracteristicaTabela = CaracteristicaHelper.GetQueries(caracteristicaTabela, idSelecao);

            IEnumerable<TabelaQuery> unionTabela = consultas.Union(consultasCaracteristicaTabela);

            Dictionary<int, IDictionary<string, object>> principal = new Dictionary<int, IDictionary<string, object>>();
            if(unionTabela.Any()){
                stopwatchPreBD.Start();
                // Busca por todas as listas de dados requisitadas.
            
                IDictionary<string, IEnumerable<object>> keyValuePairs = new Dictionary<string, IEnumerable<object>>();
                keyValuePairs = await database.GetAllData(unionTabela);

                // Separa as massas de dados em principal, para a tabela principal do setor informado e suas auxiliares.
                string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(setor);

                stopwatchPreBD.Stop();

                principal = keyValuePairs.FirstOrDefault(x => x.Key == tabelaPrincipal)
                    .Value.ToDictionary(x => (int)(x as IDictionary<string, object>)["Id"], x => (x as IDictionary<string, object>));

                // Associa as listas de dados a lista da tabela principal.
                foreach (var aux in keyValuePairs.Where(x => x.Key != tabelaPrincipal))
                {
                    foreach (var x in aux.Value.GroupBy(x => (int)(x as IDictionary<string, object>)["IdOrigem"]))
                    {
                        if (principal.TryGetValue(x.Key, out IDictionary<string, object> principalValue))
                            principalValue[aux.Key] = x.ToArray();
                    };
                }
            

            stopwatchPre.Stop();

            var auxiliares = keyValuePairs.Where(x => x.Key != tabelaPrincipal);
            var totalTabelasAuxiliares = auxiliares.Count();
            var totalRegistroTabelasAuxiliares = auxiliares.Select(x => x.Value.Select(y => y).Count()).Sum();


            Console.WriteLine("### Dados principais e relacionados\n");
            Console.WriteLine("| Medição              | Utilização       |");
            Console.WriteLine("|----------------------|------------------|");
            Console.WriteLine($"| Registros principais | {principal?.Count,-16} |");
            Console.WriteLine($"| Registros auxiliares | {totalRegistroTabelasAuxiliares,-16} |");

            Console.WriteLine($"| Memória máxima       | {(Process.GetCurrentProcess().PeakWorkingSet64 / 1024f) / 1024f + "mb",-16} |");
            Console.WriteLine($"| Tempo Banco de Dados | {stopwatchPreBD.Elapsed} |");
            Console.WriteLine($"| Tempo Total          | {stopwatchPre.Elapsed} |\n");

            }

            stopwatchPre.Stop();

            return principal;
        }

        private static void TestarFormulas(IDictionary<int, IDictionary<string, object>> dados)
        {
            int countFatorG = 0;
            int countFatorK = 0;
            int countvvt = 0;
            int countvvp = 0;
            int countIptu = 0;
            foreach (var item in dados)
            {
                var fatorG = decimal.Parse((Resultados[item.Key] as IDictionary<string, object>)["FatorG"].ToString());
                var fatorK = decimal.Parse((Resultados[item.Key] as IDictionary<string, object>)["FatorK"].ToString());
                var vvt = decimal.Parse((Resultados[item.Key] as IDictionary<string, object>)["vvt"].ToString());
                var vvp = decimal.Parse((Resultados[item.Key] as IDictionary<string, object>)["vvp"].ToString());
                var iptu = decimal.Parse((Resultados[item.Key] as IDictionary<string, object>)["IPTU"].ToString());

                var areaTerreno = decimal.Parse((item.Value as IDictionary<string, object>)["AreaTerreno"].ToString());
                if (TesteLanguage.TesteFatorG(item.Value, fatorG)) countFatorG++;
                if (TesteLanguage.TesteFatorK(fatorG, fatorK)) countFatorK++;
                if (TesteLanguage.TesteVVT(areaTerreno, fatorG, fatorK, vvt)) countvvt++;
                if (TesteLanguage.TesteVVP(areaTerreno, vvp)) countvvp++;
                if (TesteLanguage.TesteIPTU(item.Value, Resultados[item.Key], iptu)) countIptu++;
            }
            Console.WriteLine("\n## Assertividade\n");
            Console.WriteLine("| Fórmula         |  %   |");
            Console.WriteLine("|-----------------|------|");
            Console.WriteLine($"| {"Fator G",-15} | {(countFatorG / Resultados.Count) * 100:d2}% |");
            Console.WriteLine($"| {"Fator K",-15} | {(countFatorK / Resultados.Count) * 100:d3}% |");
            Console.WriteLine($"| {"VVT",-15} | {(countvvt / Resultados.Count) * 100:d3}% |");
            Console.WriteLine($"| {"VVP",-15} | {(countvvp / Resultados.Count) * 100:d3}% |");
            Console.WriteLine($"| {"IPTU",-15} | {(countIptu / Resultados.Count) * 100:d3}% |");
        }
    }
}