using Api.Database;
using Api.Dto;
using Api.Helper;
using Api.Services;
using Common.Exceptions;
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

            Console.WriteLine("\n### Resultados Pré-Processamento\n");
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
                var memory = item.Value;

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
                        if (e is LanguageException languageException) languageException.LanguageError.Source = evento.Nome;
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
            Console.WriteLine("| Resultados                     | Valor           |");
            Console.WriteLine("|--------------------------------|-----------------|");
            if (Resultados.Count <= 1)
            {
                foreach (var result in Resultados)
                    foreach (var result2 in result.Value as IDictionary<string, GenericValueLanguage>)
                    {
                        if (result2.Value.IsDictionaryIntDictionaryStringGeneric())
                            Console.WriteLine($"| {result2.Key,-30} | [ {string.Join(", ", result2.Value.AsDictionaryIntDictionaryStringGeneric().Select(x => $"{x.Key}: {{ {string.Join(", ", x.Value.Select(y => $"{y.Key}: {y.Value}"))} }}"))} ] |");
                        else if (result2.Value.IsDictionaryStringGeneric())
                            Console.WriteLine($"| {result2.Key,-30} | {{ {string.Join(", ", result2.Value.AsDictionaryStringGeneric().Select(x => $"{x.Key}: {x.Value}"))} }} |");
                        else if (result2.Value.IsDictionaryIntGeneric())
                            Console.WriteLine($"| {result2.Key,-30} | [ {string.Join(", ", result2.Value.AsDictionaryIntGeneric())} ] |");
                        else
                            Console.WriteLine($"| {result2.Key,-30} | {result2.Value,-15} |");
                    }
            }
            Console.WriteLine($"| {"Total",-30} | {Resultados.Count,-15} |");
            #endregion

            #region Erros
            if (Exceptions.Value.Count > 0)
            {
                Console.WriteLine($"\n## Erros\n");
                Console.WriteLine($"| {"Erros",-20} | {"Valor",-15} |");
                Console.WriteLine($"|----------------------|-----------------|");
                foreach (var exception in Exceptions.Value)
                    if (exception is LanguageException languageException)
                        Console.WriteLine($"Exceção: {languageException}");
                    else
                        Console.WriteLine($"Exceção: {exception.Message}");
                Console.WriteLine($"| {"Total",-20} | {Exceptions.Value.Count,-15} |");
            }
            #endregion

            // Diagnostic results
            stopwatch.Stop();
            var endProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
            double totalProcessorTime = (endProcessorTime - startProcessorTime).TotalMilliseconds;

            if (!Exceptions.Value.Any()) TestarFormulas(dados);

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

            //Adicionar no Principal e no GetAllData para trazer os valores
            var consultasCaracteristica = CaracteristicaHelper.GetQueries(caracteristica);
            //var consultasParametro = ParametroHelper.GetQueries(parametros);
            var consultaParametro = ParametroHelper.GetQuery(parametros);

            stopwatchPreBD.Start();
            IDictionary<string, IEnumerable<IDictionary<string, GenericValueLanguage>>> keyValuePairsGlobal = new Dictionary<string, IEnumerable<IDictionary<string, GenericValueLanguage>>>();
            // Busca por todas as listas de dados requisitadas.
            IEnumerable<TabelaQuery> queries = consultaParametro != null ? consultasCaracteristica.Union(new TabelaQuery[] { consultaParametro }) : consultasCaracteristica;

            await using (var database = new DatabaseConnection())
            {
                keyValuePairsGlobal = queries.Count() == 0 ? null : await database.GetAllData(queries);
            }

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
            Console.WriteLine($"| Registros globais    | {keyValuePairsGlobal?.Count ?? 0,-16} |");
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

            await using var database = new DatabaseConnection();

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
                    .ToDictionary(u => u.Where(x => x.Key == "Id").Select(x => x.Value).FirstOrDefault());

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
                    foreach (var x in aux.Value.GroupBy(x => (x as IDictionary<string, GenericValueLanguage>)["IdOrigem"]))
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

        private static void TestarFormulas(IDictionary<GenericValueLanguage, IDictionary<string, GenericValueLanguage>> dados)
        {
            int countFatorG = 0;
            int countTesteoListMemoryValue = 0;
            int countTesteContLista = 0;
            int countRetornoLista = 0;
            int countTesteRetornoVarMemoryValue = 0;
            int countUsandoFatorG = 0;
            var countCaracteristicas = new int[RoteiroService.arrayCaracteristica.Length];

            //int countFatorK = 0;
            //int countvvt = 0;
            //int countvvp = 0;
            //int countIptu = 0;

            foreach (var item in dados)
            {
                var fatorG = (Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)["FatorG"].ToString().ToNullable<decimal>();
                var testeoListMemoryValue = (Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)["TesteoListMemoryValue"].ToString().ToNullable<decimal>();
                var testeContLista = (Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)["TesteContLista"].ToString().ToNullable<decimal>();
                var retornoLista = (Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)["Retorno_Lista"].AsDictionaryIntDictionaryStringGeneric();
                var testeRetornoVarMemoryValue = (Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)["TesteRetornoVarMemoryValue"].ToString().ToNullable<decimal>();
                var usandoFatorG = (Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)["UsandoFatorG"].ToString().ToNullable<decimal>();

                for (int i = 0; i < RoteiroService.arrayCaracteristica.Length; i++)
                {
                    var caracteristicaValor = item.Value.ContainsKey($"@FisicoCaracteristicas.{RoteiroService.arrayCaracteristica[i].Replace("\"", string.Empty)}") ? item.Value[$"@FisicoCaracteristicas.{RoteiroService.arrayCaracteristica[i].Replace("\"", string.Empty)}"].AsDictionaryIntDictionaryStringGeneric()[0]["Valor"].ToString().ToNullable<decimal>() ?? 0 : 0;
                    if (TesteLanguage.TesteCaracteristica(caracteristicaValor,
                        (Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)[$"Teste{i}"].ToString().ToNullable<decimal>()))
                        countCaracteristicas[i]++;
                }

                //var fatorK = decimal.Parse((Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)["FatorK"].ToString());
                //var vvt = decimal.Parse((Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)["vvt"].ToString());
                //var vvp = decimal.Parse((Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)["vvp"].ToString());
                //var iptu = decimal.Parse((Resultados[(int)item.Key] as IDictionary<string, GenericValueLanguage>)["IPTU"].ToString());

                //var areaTerreno = decimal.Parse((item.Value as IDictionary<string, object>)["AreaTerreno"].ToString());
                if (TesteLanguage.TesteFatorG(item.Value, Resultados[(int)item.Key], fatorG)) countFatorG++;
                if (TesteLanguage.TesteoListMemoryValue(item.Value, testeoListMemoryValue)) countTesteoListMemoryValue++;
                if (TesteLanguage.TesteContLista(Resultados[(int)item.Key], testeContLista)) countTesteContLista++;
                if (TesteLanguage.RetornoLista(retornoLista)) countRetornoLista++;
                if (TesteLanguage.TesteRetornoVarMemoryValue(item.Value, Resultados[(int)item.Key], testeRetornoVarMemoryValue)) countTesteRetornoVarMemoryValue++;
                if (TesteLanguage.UsandoFatorG(Resultados[(int)item.Key], usandoFatorG)) countUsandoFatorG++;

                //if (TesteLanguage.TesteFatorK(fatorG, fatorK)) countFatorK++;
                //if (TesteLanguage.TesteVVT(areaTerreno, fatorG, fatorK, vvt)) countvvt++;
                //if (TesteLanguage.TesteVVP(areaTerreno, vvp)) countvvp++;
                //if (TesteLanguage.TesteIPTU(item.Value, Resultados[item.Key], iptu)) countIptu++;
            }
            Console.WriteLine("\n## Assertividade\n");
            Console.WriteLine("| Fórmula                        |  %   |");
            Console.WriteLine("|--------------------------------|------|");
            Console.WriteLine($"| {"FatorG",-30} | {(countFatorG / Resultados.Count) * 100:d2}% |");
            Console.WriteLine($"| {"TesteoListMemoryValue",-30} | {(countTesteoListMemoryValue / Resultados.Count) * 100:d2}% |");
            Console.WriteLine($"| {"TesteContLista",-30} | {(countTesteContLista / Resultados.Count) * 100:d2}% |");
            Console.WriteLine($"| {"Retorno_Lista",-30} | {(countRetornoLista / Resultados.Count) * 100:d2}% |");
            Console.WriteLine($"| {"TesteRetornoVarMemoryValue",-30} | {(countTesteRetornoVarMemoryValue / Resultados.Count) * 100:d2}% |");
            Console.WriteLine($"| {"UsandoFatorG",-30} | {(countUsandoFatorG / Resultados.Count) * 100:d2}% |");
            for (int i = 0; i < countCaracteristicas.Length; i++)
            {
                Console.WriteLine($"| {$"Teste{i}",-30} | {(countCaracteristicas[i] / Resultados.Count) * 100:d2}% |");
            }

            //Console.WriteLine($"| {"Fator K",-22} | {(countFatorK / Resultados.Count) * 100:d3}% |");
            //Console.WriteLine($"| {"VVT",-22} | {(countvvt / Resultados.Count) * 100:d3}% |");
            //Console.WriteLine($"| {"VVP",-22} | {(countvvp / Resultados.Count) * 100:d3}% |");
            //Console.WriteLine($"| {"IPTU",-22} | {(countIptu / Resultados.Count) * 100:d3}% |");
        }
    }
}