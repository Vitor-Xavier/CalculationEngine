using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Api.Dto;

namespace Api
{
    public class Program
    {

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
                var valor = 0;
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
                Formula = @"retorno @Roteiro.FatorG * @Roteiro.FatorK * @Fisico.AreaTerreno;"
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

            var listaFisico = new List<ExemploFisico>();

            for (int i = 0; i < 100; i++)
            {
                var novoFisico = new ExemploFisico()
                {
                    Id = 0,
                    AreaEdificada = GerarDecimal(),
                    AreaTerreno = GerarDecimal(),
                    Caracteristica = GerarDecimal(),
                    CaracteristicaEspecial = GerarDecimal(),
                    Conservacao = GerarDecimal(),
                    FatorPosicaoQuadra = GerarDecimal(),
                    FracaoIdeal = GerarDecimal(),
                    LocalPropriedadeLote = GerarDecimal(),
                    NumeroFrentes = GerarDecimal(),
                    Pedologia = GerarDecimal(),
                    Pontos = GerarDecimal(),
                    Testada = GerarDecimal(),
                    Topografia = GerarDecimal(),
                    ValorM = GerarDecimal(),

                    Teste01 = GerarDecimal(),
                    Teste02 = GerarDecimal(),
                    Teste03 = GerarDecimal(),
                    Teste04 = GerarDecimal(),
                    Teste05 = GerarDecimal(),
                    Teste06 = GerarDecimal(),
                    Teste07 = GerarDecimal(),

                };
                listaFisico.Add(novoFisico);
            }

            // Diagnostic tools
            var process = Process.GetCurrentProcess();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Pré-Processamento
            Console.WriteLine("..Pré-Processamento");
            var grupo = new List<TabelaColuna>();
            List<CaracteristicaParametros> caracteristicaParametros = new List<CaracteristicaParametros>();

            foreach (var evento in roteiro.Eventos)
            {
                ExecuteLanguage execute = new ExecuteLanguage();
                execute.DefaultParserTree(evento.Formula);

                var tokens = execute.commonToken.GetTokens();
                var tokenTypeMap = execute.parser.TokenTypeMap;
                caracteristicaParametros = ListaCaracteristicaParametros(tokens, tokenTypeMap);

                grupo.AddRange(ListaTabelaColuna(tokens, tokenTypeMap));
            }
            grupo = grupo.GroupBy(t => t.Tabela).Select(x => new TabelaColuna { Tabela = x.Key, Coluna = x.SelectMany(y => y.Coluna).Distinct().ToList() }).ToList();

            var keyValuePairs = new Dictionary<string, IList<object>>();
            foreach (var item in grupo)
            {
                var sql = GetDefaultSQL(item);
                keyValuePairs.Add(item.Tabela, await GetData(sql, item.Coluna));
            }


            var stopWatchProcessamento = new Stopwatch();
            stopWatchProcessamento.Start();
            Console.WriteLine("..Processamento");
            // Processamento
            var exceptions = new ConcurrentQueue<Exception>();
            var results = new ConcurrentDictionary<int, object>();
            var first = keyValuePairs.FirstOrDefault();
            var last = roteiro.Eventos.LastOrDefault();
            Parallel.ForEach(first.Value, new ParallelOptions { MaxDegreeOfParallelism = 100 }, item =>
            {
                // Memory
                (item as IDictionary<string, object>).TryGetValue("Id", out object objId);
                int itemId = (int)objId;
                var memory = (item as IDictionary<string, object>).ToDictionary(x => $"@{first.Key}.{x.Key}", y => new GenericValueLanguage(y.Value));

                // TODO: Itens auxiliares ao Memory
                //var auxMemory = keyValuePairs.Skip(1).Select(x => new { Table = x.Key, Values = x.Value.Where(y => (y as IDictionary<string, object>).TryGetValue("IdOrigem", out object idOrigem) && ((int)idOrigem) == itemId) });
                //foreach (var m in auxMemory)
                //{
                //    foreach (var v in m.Values)
                //    {
                //        (v as IDictionary<string, object>)
                //        .Select(x => new { Key = $"@{m.Table}.{x.Key}", Value = new GenericValueLanguage(x.Value) })
                //        .ToList().ForEach(item =>
                //        {
                //            memory.Add(item.Key, item.Value);
                //        });
                //    }
                //}

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
                        exceptions.Enqueue(e);
                    }
                });
                if (memory.TryGetValue($"@Roteiro.{last.Nome}", out GenericValueLanguage genericValue))
                    results.TryAdd(itemId, genericValue.Value);
            });
            stopWatchProcessamento.Stop();
            Console.WriteLine($"Total: {stopwatch.Elapsed}");

            // Resultados
            Console.WriteLine("..Resultados");
            if (results.Any())
            {
                Console.WriteLine($"-- Resultados {results.Count}");
                if (results.Count <= 20)
                    foreach (var result in results)
                        Console.WriteLine($"{first.Key} {result.Key}: {result.Value}");
            }
            else
                Console.WriteLine("Sem resultados");

            // Erros
            if (exceptions.Count > 0)
            {
                Console.WriteLine("-- Erros");
                foreach (var exception in exceptions)
                    Console.WriteLine($"Exceção: {exception.Message}");
            }

            // Diagnostic results
            stopwatch.Stop();
            Console.WriteLine($"Tempo Total: {stopwatch.Elapsed}");
            Console.WriteLine($"Memória máxima utilizada: {(process.PeakWorkingSet64 / 1024f) / 1024f}mb");
        }

        private static async Task<List<object>> GetData(string sql, IEnumerable<string> colunas)
        {
            List<object> list = new List<object>();
            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                connection = new SqlConnection("");
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sql, connection);
                reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var obj = new ExpandoObject() as IDictionary<string, object>;
                    if (!colunas.Contains("Id"))
                        obj.Add("Id", reader["Id"]);
                    if (HasColumn(reader, "IdOrigem") && !colunas.Contains("IdOrigem"))
                        obj.Add("IdOrigem", reader["IdOrigem"]);
                    foreach (var coluna in colunas.Distinct())
                        obj.Add(coluna, reader[coluna]);
                    list.Add(obj);
                }
            }
            finally
            {
                reader?.Close();
                connection?.Close();
            }
            return list;
        }

        public static bool HasColumn(SqlDataReader reader, string columnName)
        {
            foreach (DataRow row in reader.GetSchemaTable().Rows)
            {
                if (row["ColumnName"].ToString() == columnName)
                    return true;
            }
            return false;
        }

        private static string GetDefaultSQL(TabelaColuna tabela) =>
            tabela.Tabela switch
            {
                "Fisico" => $"SELECT IdFisico as Id, {string.Join(", ", tabela.Coluna)} FROM Fisico ORDER BY IdFisico",
                "FisicoOutros" => $"SELECT TOP 10 IdFisicoOutro as Id, IdFisico as IdOrigem, {string.Join(", ", tabela.Coluna)} FROM FisicoOutros ORDER BY IdFisico",
                "FisicoRural" => $"SELECT TOP 10 IdFisicoRural as Id, {string.Join(", ", tabela.Coluna)} FROM FisicoRural ORDER BY IdFisicoRural",
                _ => throw new Exception("Tabela não configurado para o cálculo")
            };

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
                tabela = x.Text.RemoveCaracter("@").SubstringWithIndexOf('.'),
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