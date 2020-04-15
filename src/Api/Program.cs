using Api.Database;
using Api.Dto;
using Api.Helper;
using Api.Services;
using Common.Extensions;
using Implementation;
using System;
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
        public static Lazy<ConcurrentQueue<Exception>> Exceptions { get; } = new Lazy<ConcurrentQueue<Exception>>(() => new ConcurrentQueue<Exception>());

        public static ConcurrentDictionary<int, object> Resultados { get; } = new ConcurrentDictionary<int, object>();

        static async Task Main()
        {
            Console.WriteLine("# Cálculo Tributário");
            Console.WriteLine("\n## Roteiro\n");
            var roteiroService = new RoteiroService();
            var roteiro = await roteiroService.GetRoteiro();
            Console.WriteLine("| Roteiro  | Valor      |");
            Console.WriteLine("|----------|------------|");
            Console.WriteLine($"| Nome     | {roteiro.Nome,-10} |");
            Console.WriteLine($"| Fórmulas | {roteiro.Eventos.Count,-10} |");

            // Ferramentas Diagnóstico
            var stopwatch = new Stopwatch();
            var startProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
            stopwatch.Start();

            #region PreProcessamento
            Console.WriteLine("\n## Pré-Processamento\n");
            var stopwatchPreProcessamento = new Stopwatch();
            var cpuPreProcessamentoStart = Process.GetCurrentProcess().TotalProcessorTime;
            stopwatchPreProcessamento.Start();

            var tabelas = TabelaColunaHelper.GetTabelaColunas(roteiro);
            var caracteristicaTabela = TabelaColunaHelper.GetCaracteristicaTabela(roteiro.Eventos);
            var atividadeTabela = TabelaColunaHelper.GetAtividadeTabela(roteiro.Eventos);
            var caracteristica = TabelaColunaHelper.GetCaracteristica(roteiro.Eventos);
            var parametros = TabelaColunaHelper.GetParametros(roteiro.Eventos);

            var dados = await CarregarDados(roteiro.SetorOrigem, tabelas, caracteristicaTabela, atividadeTabela);
            var memoryGlobal = await CarregarDadosGlobal(caracteristica, parametros);

            // Avaliação das fórmulas
            var executeLanguage = new ExecuteLanguage();
            foreach (var evento in roteiro.Eventos)
                evento.ParseTree = executeLanguage.DefaultParserTree(evento.Formula);

            // Verifica se erros de sintaxe foram identificados.
            if (executeLanguage.LanguageErrorListener.SyntaxErrors.Any())
            {
                Console.WriteLine("\n## Erros de Sintaxe");
                foreach (var error in executeLanguage.LanguageErrorListener.SyntaxErrors)
                    Console.WriteLine($"| Linha: {error.Line} | Coluna: {error.StartColumn} | Caractere: {error.OffendingSymbol} | Mensagem: {error.Message} |");
                return;
            }

            stopwatchPreProcessamento.Stop();
            var cpuPreProcessamentoEnd = Process.GetCurrentProcess().TotalProcessorTime;
            double cpuTotalPreProcessamento = (cpuPreProcessamentoEnd - cpuPreProcessamentoStart).TotalMilliseconds;

            Console.WriteLine("\n## Resultados Pré-Processamento\n");
            Console.WriteLine("| Medição        | Utilização       |");
            Console.WriteLine("|----------------|------------------|");
            Console.WriteLine($"| Tempo Total    | {stopwatchPreProcessamento.Elapsed} |");
            Console.WriteLine($"| CPU média      | {Math.Round((cpuTotalPreProcessamento / (Environment.ProcessorCount * stopwatchPreProcessamento.ElapsedMilliseconds)) * 100) + "%",-16} |\n");
            #endregion

            #region Processamento
            Console.WriteLine("\n## Processamento\n");

            var stopWatchProcessamento = new Stopwatch();
            var cpuProcessamentoStart = Process.GetCurrentProcess().TotalProcessorTime;
            stopWatchProcessamento.Start();

            // Principal 
            string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(roteiro.SetorOrigem);
            Parallel.ForEach(dados, item =>
            {
                //var aux = item.Value.Where(x => x.Value is object[] || x.Value is ExpandoObject);

                //  var aux2 = item.Value.Where(x => x.Any(u => u.Value is IDictionary<string, GenericValueLanguage>));

                Dictionary<string, GenericValueLanguage> aux = new Dictionary<string, GenericValueLanguage>();
                //var memory = item.Value.ToDictionary(x => $"@{x.Key}", x => new GenericValueLanguage(x.Value));
                //var objPrincipal = item.Value.Except(aux).Aggregate(new ExpandoObject() as IDictionary<string, object>, (a, p) => { a.Add(p.Key, p.Value); return a; });
                var memory = item.Value;
                var objPrincipal = item.Value;
                //memory.Add($"@{tabelaPrincipal}", new GenericValueLanguage(objPrincipal));
                memory.Add("@Roteiro", new GenericValueLanguage(new Dictionary<string, GenericValueLanguage>()));

                // Execucao das Formulas do Roteiro
                var executeFormula = new ExecuteLanguage(memoryGlobal);
                foreach (var evento in roteiro.Eventos)
                {
                    try
                    {
                        // Execução
                        var result = executeFormula.Execute(memory, evento.ParseTree);

                        // Resultado
                        (memory["@Roteiro"].Value as IDictionary<string, GenericValueLanguage>).Add(evento.Nome, new GenericValueLanguage(result.Value));
                    }
                    catch (Exception e)
                    {
                        Exceptions.Value.Enqueue(e);
                        break;
                    }
                };
                
                Resultados.TryAdd((int)item.Key, memory["@Roteiro"].Value);
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
                    foreach (var result2 in result.Value as IDictionary<string, GenericValueLanguage>)
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

            //if (!Exceptions.Value.Any()) TestarFormulas(dados);

            Console.WriteLine("\n## Geral\n");
            Console.WriteLine("| Medição        | Utilização       |");
            Console.WriteLine("|----------------|------------------|");
            Console.WriteLine($"| Tempo Total    | {stopwatch.Elapsed} |");
            Console.WriteLine($"| Memória máxima | {(Process.GetCurrentProcess().PeakWorkingSet64 / 1024f) / 1024f + "mb",-16} |");
            Console.WriteLine($"| CPU média      | {Math.Round((totalProcessorTime / (Environment.ProcessorCount * stopwatch.ElapsedMilliseconds)) * 100) + "%",-16:d2} |\n");
        }



        public static async Task<IDictionary<string, GenericValueLanguage>> CarregarDadosGlobal(IEnumerable<Caracteristica> caracteristica, IEnumerable<Parametro> parametros)
        {

            // Diagnóstico
            var stopwatchPre = new Stopwatch();
            stopwatchPre.Start();

            var stopwatchPreBD = new Stopwatch();

            // TODO: BuscaCaracteristica GroupBy

            var database = new DatabaseConnection();


            //Adicionar no Principal e no GetAllData para trazer os valores
            var consultasCaracteristica = CaracteristicaHelper.GetQueries(caracteristica);
            //var consultasParametro = ParametroHelper.GetQueries(parametros);
            var consultaParametro = ParametroHelper.GetQuery(parametros);

            stopwatchPreBD.Start();
            IDictionary<string, IEnumerable<IDictionary<string, GenericValueLanguage>>> keyValuePairsGlobal = new Dictionary<string, IEnumerable<IDictionary<string, GenericValueLanguage>>>();
            // Busca por todas as listas de dados requisitadas.
            IEnumerable<TabelaQuery> queries = consultaParametro != null ? consultasCaracteristica.Union(new TabelaQuery[] { consultaParametro }) : consultasCaracteristica;
            keyValuePairsGlobal = queries.Count() == 0 ? null : await database.GetAllData(queries);

            // Separa as massas de dados em principal, para a tabela principal do setor informado e suas auxiliares.

            var tabelaGlobal = SetorOrigemHelper.GetTabelasSetor(SetorOrigem.Global);

            var global = keyValuePairsGlobal != null && keyValuePairsGlobal.Count != 0 ? keyValuePairsGlobal.ToDictionary(x => $"@{x.Key}", x => new GenericValueLanguage(x.Value)) : null;
            var global2 = keyValuePairsGlobal != null && keyValuePairsGlobal.Count != 0 ? keyValuePairsGlobal.ToDictionary(x => $"@{x.Key}", x => x.Value) : null;

            Dictionary<GenericValueLanguage, IDictionary<string, GenericValueLanguage>> principal = new Dictionary<GenericValueLanguage, IDictionary<string, GenericValueLanguage>>();
            IDictionary<int, IDictionary<string, GenericValueLanguage>> array = new Dictionary<int, IDictionary<string, GenericValueLanguage>>();


            foreach (var item in global2)
            {
                int i = 0;
                foreach (var x in item.Value)
                {
                    array[i] = x;
                    i++;
                }
                global[item.Key] = new GenericValueLanguage(array);
            }

            stopwatchPreBD.Stop();

            stopwatchPre.Stop();

            Console.WriteLine("### Dados Globais\n");
            Console.WriteLine("| Medição              | Utilização       |");
            Console.WriteLine("|----------------------|------------------|");
            Console.WriteLine($"| Registros globais    | {(keyValuePairsGlobal is null ? 0 : keyValuePairsGlobal.Count),-16} |");
            Console.WriteLine($"| Tempo Banco de Dados | {stopwatchPreBD.Elapsed} |");
            Console.WriteLine($"| Tempo Total          | {stopwatchPre.Elapsed} |");

            return global;
        }

        public static async Task<Dictionary<GenericValueLanguage, IDictionary<string, GenericValueLanguage>>> CarregarDados(SetorOrigem setor, IEnumerable<TabelaColuna> tabelas, IEnumerable<CaracteristicaTabela> caracteristicaTabela, List<AtividadeTabela> atividadeTabela)
        {
            int idSelecao = 2;

            // Diagnóstico
            var stopwatchPre = new Stopwatch();
            stopwatchPre.Start();

            var stopwatchPreBD = new Stopwatch();

            // TODO: BuscaCaracteristica GroupBy

            if (!SetorOrigemHelper.ValidarTabelasSetor(setor, tabelas.Select(g => g.Tabela)))
                throw new Exception($"Erro ao validar tabelas utilizadas para o setor {setor.GetDescription()}");

            var database = new DatabaseConnection();

            IEnumerable<TabelaQuery> consultas = Enumerable.Empty<TabelaQuery>();
            if (tabelas.Any() && tabelas.Count() > 0)
                consultas = SetorOrigemHelper.GetQueries(setor, tabelas, idSelecao);

            IEnumerable<TabelaQuery> consultasCaracteristicaTabela = Enumerable.Empty<TabelaQuery>();
            if (caracteristicaTabela.Any() && tabelas.Count() > 0)
                consultasCaracteristicaTabela = CaracteristicaHelper.GetQueries(caracteristicaTabela, idSelecao);


            IEnumerable<TabelaQuery> consultasAtividadeTabelas = Enumerable.Empty<TabelaQuery>();
            if (atividadeTabela.Any() && atividadeTabela.Count() > 0)
            {
                var query = DataBaseHelper.GetDefaultKeySql(atividadeTabela.Select(x => "'" + x.Tabela + "'").Distinct().ToList());
                var result = await database.Sql(query);
                consultasAtividadeTabelas = AtividadeHelper.GetQueries(idSelecao, atividadeTabela.AsEnumerable(), result);
            }

            IEnumerable<TabelaQuery> unionTabela = consultas.Union(consultasCaracteristicaTabela).Union(consultasAtividadeTabelas);


            Dictionary<GenericValueLanguage, IDictionary<string, GenericValueLanguage>> principal = new Dictionary<GenericValueLanguage, IDictionary<string, GenericValueLanguage>>();
            Dictionary<GenericValueLanguage, IDictionary<string, GenericValueLanguage>> principalNovo = new Dictionary<GenericValueLanguage, IDictionary<string, GenericValueLanguage>>();
            IDictionary<string, IEnumerable<IDictionary<string, GenericValueLanguage>>> keyValuePairs = new Dictionary<string, IEnumerable<IDictionary<string, GenericValueLanguage>>>();
            if (unionTabela.Any())
            {
                stopwatchPreBD.Start();
                // Busca por todas as listas de dados requisitadas.

                // Separa as massas de dados em principal, para a tabela principal do setor informado e suas auxiliares.
                string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(setor);

                keyValuePairs = await database.GetAllData(unionTabela);
                IDictionary<string, IEnumerable<IDictionary<string, GenericValueLanguage>>> principalTeste = new Dictionary<string, IEnumerable<IDictionary<string, GenericValueLanguage>>>();

                principal = keyValuePairs.FirstOrDefault(x => x.Key == tabelaPrincipal).Value
                .ToDictionary(u => u.Where(x => x.Key == "Id")
                .Select(x => x.Value)
                .FirstOrDefault());

                foreach (var item in principal)
                {
                    var memory = item.Value.ToDictionary(x =>
                    string.IsNullOrEmpty(x.Key.Substring(0, x.Key.IndexOf(".", StringComparison.Ordinal) < 0 ? 0 : x.Key.IndexOf(".", StringComparison.Ordinal))) ?
                    $"@{tabelaPrincipal}.{x.Key}" : $"@{x.Key}", x => x.Value);
                    principalNovo[item.Key] = memory;
                }

                principal = principalNovo;

                stopwatchPreBD.Stop();

                foreach (var aux in keyValuePairs.Where(x => x.Key != tabelaPrincipal))
                {
                    foreach (var x in aux.Value.GroupBy(x => (GenericValueLanguage)(x as IDictionary<string, GenericValueLanguage>)["IdOrigem"]))
                    {

                        if (principal.TryGetValue(x.Key, out IDictionary<string, GenericValueLanguage> principalValue2))
                        {
                            int i = 0;
                            IDictionary<int, IDictionary<string, GenericValueLanguage>> array = new Dictionary<int, IDictionary<string, GenericValueLanguage>>();
                            foreach (var itemTeste in x)
                            {
                                array.Add(i, itemTeste);
                                i++;
                            }
                            principalValue2[$"@{aux.Key}"] = new GenericValueLanguage(array);
                        }

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