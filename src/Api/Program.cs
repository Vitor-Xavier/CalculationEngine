using Antlr4.Runtime;
using Api.Database;
using Api.Dto;
using Api.Extensions;
using Api.Helper;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api
{
    public class Program
    {
        public static SetorOrigem SetorOrigem { get; set; }

        public static Lazy<ConcurrentQueue<Exception>> Exceptions { get; } = new Lazy<ConcurrentQueue<Exception>>(() => new ConcurrentQueue<Exception>());

        public static ConcurrentDictionary<int, object> Resultados { get; } = new ConcurrentDictionary<int, object>();

        static decimal GerarDecimal()
        {
            Random rnd = new Random();
            byte scale = (byte)rnd.Next(29);
            bool sign = rnd.Next(2) == 1;
            var teste = new decimal(rnd.Next(10), rnd.Next(10), rnd.Next(10), sign, scale);
            return teste;
        }

        static async Task Main(string[] args)
        {
            SetorOrigem = SetorOrigem.Imobiliario;

            Console.WriteLine("-- Roteiro");
            var roteiro = GetRoteiro();

            // Ferramentas Diagnóstico
            var process = Process.GetCurrentProcess();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("\n-- Pré-Processamento");
            var keyValuePairs = await CarregarDados(SetorOrigem, roteiro);

            #region Processamento
            Console.WriteLine("\n-- Processamento");

            var stopWatchProcessamento = new Stopwatch();
            stopWatchProcessamento.Start();

            // Principal 
            string tabelaPrincipal = SetorOrigemHelper.GetTabelaPrincipal(SetorOrigem);
            var listaPrincipal = keyValuePairs.FirstOrDefault(x => x.Key == tabelaPrincipal);

            var listasAuxiliares = keyValuePairs.Where(x => x.Key != tabelaPrincipal).ToDictionary(x => x.Key, x => x.Value);

            var stopWatchMemory = new Stopwatch();
            Parallel.ForEach(listaPrincipal.Value, new ParallelOptions { MaxDegreeOfParallelism = 1 }, item =>
            {
                // Memory
                stopWatchMemory.Start();
                int itemId = (int) (item as IDictionary<string, object>)["Id"];
                var memory = (item as IDictionary<string, object>).ToDictionary(x => $"@{listaPrincipal.Key}.{x.Key}", y => new GenericValueLanguage(y.Value));

                // TODO: Itens auxiliares ao Memory
                var itemAuxiliares = listasAuxiliares
                    .ToDictionary(x => x.Key, x => x.Value.Where(y => (int) (y as IDictionary<string, object>)["IdOrigem"] == itemId));

                // Mapear os 'Filhos' jogando na Memory
                foreach (var aux in itemAuxiliares)
                {
                    int i = 0;
                    foreach (IDictionary<string, object> value in aux.Value)
                    {
                        value.ToList().ForEach(item =>
                        {
                            memory.Add($"@{aux.Key}[{i}].{item.Key}", new GenericValueLanguage(item.Value));
                        });
                        i++;
                    }
                }
                stopWatchMemory.Stop();

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
                Resultados.TryAdd(itemId, resultado);
            });
            stopWatchProcessamento.Stop();
            Console.WriteLine($"Tempo Total Manipulando 'memory': {stopWatchMemory.Elapsed}");
            Console.WriteLine($"Tempo Total Processamento: {stopWatchProcessamento.Elapsed}");
            #endregion

            #region Resultados
            Console.WriteLine($"\n-- Resultados {Resultados.Count}");
            if (Resultados.Count <= 20)
                foreach (var result in Resultados)
                    Console.WriteLine($"{listaPrincipal.Key} {result.Key}: {result.Value}");
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

        public static Roteiro GetRoteiro()
        {
            Roteiro roteiro = new Roteiro
            {
                Id = 1,
                NomeRoteiro = "IPTU",
                SetorOrigem = "Imobiliario",
                Eventos = new List<Evento>()
            };

            Evento fatorG = new Evento
            {
                Id = 1,
                Nome = "FatorG",
                Formula = @"
                var valor = 0.0;
                var area = _COALESCE(@FisicoAreas.Area, @FisicoOutros.Percentual, 9.0);
                var percentual = @FisicoOutros.Percentual;
                se (@Fisico.AreaEdificada > 0.0) {
                    valor = @Fisico.AreaEdificada * 1.05;
                } senao {
                    valor = @Fisico.AreaEdificada * @Fisico.Testada;
                }
                retorno valor * 10;"
            };

            Evento fatorK = new Evento
            {
                Id = 2,
                Nome = "FatorK",
                Formula = @"
                var valor = 0.0;
                se (@Roteiro.FatorG > 5725.90) {
                    valor = 5725.90;
                } senao {
                    valor = 3775.90;
                }
                retorno valor;"
            };

            Evento vvt = new Evento
            {
                Id = 3,
                Nome = "vvt",
                Formula = @"retorno @Roteiro.FatorG * @Roteiro.FatorK + @Fisico.AreaTerreno;"
            };

            Evento vvp = new Evento
            {
                Id = 4,
                Nome = "vvp",
                Formula = @"retorno @Fisico.AreaTerreno * 100;"
            };

            Evento iptu = new Evento
            {
                Id = 5,
                Nome = "IPTU",
                Formula = @"retorno @Roteiro.vvp + @Roteiro.vvt;"
            };

            roteiro.Eventos.Add(fatorG);
            roteiro.Eventos.Add(fatorK);
            roteiro.Eventos.Add(vvt);
            roteiro.Eventos.Add(vvp);
            roteiro.Eventos.Add(iptu);

            return roteiro;
        }

        public static async Task<IDictionary<string, IEnumerable<object>>> CarregarDados(SetorOrigem setor, Roteiro roteiro)
        {
            // Diagnóstico
            var stopwatchPre = new Stopwatch();
            stopwatchPre.Start();

            var grupo = new List<TabelaColuna>();
            List<CaracteristicaParametros> caracteristicaParametros = new List<CaracteristicaParametros>();

            foreach (var evento in roteiro.Eventos)
            {
                ExecuteLanguage execute = new ExecuteLanguage();
                execute.DefaultParserTree(evento.Formula);

                var tokens = execute.commonToken.GetTokens();
                var tokenTypeMap = execute.parser.TokenTypeMap;

                // Separa Massa de Dados para Buscar no Banco de Dados
                caracteristicaParametros = ListaCaracteristicaParametros(tokens, tokenTypeMap);
                grupo.AddRange(ListaTabelaColuna(tokens, tokenTypeMap));

            }
            grupo = grupo.GroupBy(t => t.Tabela).Select(x => new TabelaColuna { Tabela = x.Key, Coluna = x.SelectMany(y => y.Coluna).Distinct().ToList() }).ToList();
            if (!SetorOrigemHelper.ValidarTabelasSetor(setor, grupo.Select(g => g.Tabela)))
                throw new Exception($"Erro ao validar tabelas utilizadas para o setor {setor.GetDescription()}");

            var database = new DatabaseConnection();

            // Get Massa de Dados
            string sql = null;
            foreach (var item in grupo)
                sql += SetorOrigemHelper.GetDefaultSQL(item);

            var keyValuePairs = await database.GetAllData(grupo.Select(x => x.Tabela).ToArray(), sql);

            stopwatchPre.Stop();
            Console.WriteLine($"Tempo Total Pré-Processamento: {stopwatchPre.Elapsed}");

            return keyValuePairs;
        }

        private static List<CaracteristicaParametros> ListaCaracteristicaParametros(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
        {
            var tokenBuscarCaracteristica = TokenTypeMap.Where(x => x.Key == "BUSCAR_CARACTERISTICA").FirstOrDefault();
            var tokenRParen = TokenTypeMap.Where(x => x.Key == "RPAREN").FirstOrDefault();
            var valueTokenTableCaracteristica = TokenTypeMap.Where(x => x.Key == "IDENTIFIER").Select(x => x.Value).FirstOrDefault();
            bool tokensEOF = true;
            int tokenIndex = 0;
            List<CaracteristicaParametros> caracteristicaParametrosList = new List<CaracteristicaParametros>();
            while (tokensEOF)
            {
                var getTokenBuscarCaracteristica = Tokens.Where(x => x.Type == tokenBuscarCaracteristica.Value && x.TokenIndex > tokenIndex).FirstOrDefault();
                var getTokenRParen = Tokens.Where(x => x.Type == tokenRParen.Value && x.TokenIndex > tokenIndex).FirstOrDefault();
                if (getTokenBuscarCaracteristica is null || getTokenRParen is null)
                {
                    tokensEOF = false;
                    continue;
                }
                int indexTokenBuscarCaracteristica = getTokenBuscarCaracteristica.TokenIndex;
                int indexTokenRParen = getTokenRParen.TokenIndex;
                var rangeTokenBuscaCaracteristica = Tokens.ToArray()[indexTokenBuscarCaracteristica..indexTokenRParen];
                CaracteristicaParametros caracteristicaValores = new CaracteristicaParametros()
                {
                    TabelaCaracteristica = ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 0),
                    DescricaoCaracteristica = ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 1),
                    ValorFatorCaracteristica = ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 2),
                    ExercicioCaracteristica = ExtractTextToken(valueTokenTableCaracteristica, rangeTokenBuscaCaracteristica, 3),
                };
                caracteristicaParametrosList.Add(caracteristicaValores);
                tokenIndex = getTokenRParen.TokenIndex;
            }
            return caracteristicaParametrosList;
        }

        private static string ExtractTextToken(int valueTokenTableCaracteristica, IToken[] rangeTokenBuscaCaracteristica, int ordem)
        {
            var tokenCaracteristica = rangeTokenBuscaCaracteristica.Where(x => x.Type == valueTokenTableCaracteristica).ToArray();
            return tokenCaracteristica.Length > ordem ? tokenCaracteristica[ordem].Text : string.Empty;

        }

        private static List<TabelaColuna> ListaTabelaColuna(IList<IToken> Tokens, IDictionary<string, int> TokenTypeMap)
        {
            List<TabelaColuna> grupo;
            var tokenVarTableColuna = TokenTypeMap.Where(x => x.Key == "VAR_TABLE_COLUNA").FirstOrDefault();
            var tokensTypeVarTableColuna = Tokens.Where(x => x.Type == tokenVarTableColuna.Value).ToList();
            grupo = tokensTypeVarTableColuna.Select(x => new
            {
                tabela = Regex.Replace(x.Text.RemoveCaracter("@").SubstringWithIndexOf('.'), @"\[[0-9]+\]", string.Empty),
                coluna = x.Text.RemoveCaracter("@").SubstringWithIndexOf('.', true).RemoveCaracter(".")
            })
                .Where(u => u.tabela != "Roteiro")
                .GroupBy(u => u.tabela)
                .Select(y => new TabelaColuna()
                {
                    Tabela = y.Select(y => y.tabela).Distinct().FirstOrDefault(),
                    Coluna = y.Select(y => y.coluna).ToList()
                })
                .ToList();
            return grupo;
        }
    }
}