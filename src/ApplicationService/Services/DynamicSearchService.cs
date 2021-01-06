using ApplicationService.Comparers;
using Common.Enums;
using Common.Exceptions;
using Common.Helpers;
using Core.Helpers;
using Core.Implementation;
using Crosscutting.DTO.DynamicSearch;
using Crosscutting.DTO.Formula;
using Crosscutting.DTO.Roteiro;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationService.Services
{
    public class DynamicSearchService
    {
        private readonly ContribuinteRepository _contribuinteRepository;

        private readonly FisicoRepository _fisicoRepository;

        private readonly FaceQuadraRepository _faceQuadraRepository;

        private readonly DynamicSearchRepository _dynamicSearchRepository;

        public DynamicSearchService(DynamicSearchRepository dynamicSearchRepository,
                                    ContribuinteRepository contribuinteRepository,
                                    FisicoRepository fisicoRepository,
                                    FaceQuadraRepository faceQuadraRepository)
        {
            _fisicoRepository = fisicoRepository;
            _contribuinteRepository = contribuinteRepository;
            _faceQuadraRepository = faceQuadraRepository;
            _dynamicSearchRepository = dynamicSearchRepository;
        }

        public Task<IDictionary<int, IDictionary<string, GenericValueLanguage>>> SearchDataFormula(FormulaDto formula, int selecaoId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var execute = new ExecuteLanguage();

            var tokens = execute.GetTokens(formula.Texto);
            var tables = LanguageHelper.GetTables(tokens).ToList();

            return SearchData(tables, formula.SetorOrigem, selecaoId, cancellationToken);
        }

        public Task<IDictionary<int, IDictionary<string, GenericValueLanguage>>> SearchDataRoteiro(RoteiroDto roteiro, int selecaoId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var execute = new ExecuteLanguage();

            var tables = roteiro.Formulas.SelectMany(f =>
            {
                var tokens = execute.GetTokens(f.Texto);
                return LanguageHelper.GetTables(tokens).ToList();
            }).GroupBy(t => t.Name).Select(t => new Table
            {
                Name = t.Key,
                Columns = t.SelectMany(c => c.Columns).Select(c => c.Name).Distinct().Select(c => new Column { Name = c }).ToList()
            }).ToList();

            return SearchData(tables, roteiro.SetorOrigem, selecaoId, cancellationToken);
        }

        public async Task<IDictionary<int, IDictionary<string, GenericValueLanguage>>> SearchData(ICollection<Table> tables, SetorOrigem setorOrigem, int selecaoId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string principal = SetorOrigemHelper.GetTabelaPrincipal(setorOrigem);
            var relacionadasPrincipal = SetorOrigemHelper.GetTabelasRelacionadas(setorOrigem).Select(r => _dynamicSearchRepository.GetTable(r));

            // Verifica a fórmula possui tabelas válidas e do mesmo Setor Origem
            var tabelasSetor = SetorOrigemHelper.GetTabelasSetor(setorOrigem);
            if (tables.Any(t => !tabelasSetor.Contains(t.Name)))
                throw new BadRequestException("Existem tabelas não configuradas ou se Setor Origem diferentes do informado na Fórmula");

            // Verifica se a tabela principal está presente na fórmula, e caso não esteja, adiciona
            if (!tables.Any(t => t.Name == principal))
                tables.Add(new Table { Name = principal });

            var entities = tables.Select(table =>
            {
                var entity = _dynamicSearchRepository.GetTable(table.Name);

                // Verificação da presença de chave principal na busca
                if (!table.Columns.Any(c => c.IsPrimaryKey))
                    table.Columns.Add(entity.Columns.FirstOrDefault(c => c.IsPrimaryKey));

                // Verifica se a chave estrangeira de tabelas relacionadas estão presentes, caso estas tabelas sejam utilizadas
                if (table.Name == principal)
                {
                    foreach (var t in tables.Where(t => relacionadasPrincipal.Any(r => r.Name == t.Name)))
                    {
                        var primary = t.Columns.FirstOrDefault(pk => pk.IsPrimaryKey);
                        if (!table.Columns.Any(c => c.Name == primary.Name))
                            table.Columns.Add(entity.Columns.FirstOrDefault(x => x.Name == primary.Name));
                    }
                }

                ValidateSearch(table, entity);

                entity.Columns = entity.Columns.Intersect(table.Columns, new ColumnComparer()).ToList();
                return entity;
            });

            var tableData = new Dictionary<string, IDictionary<int, IDictionary<string, GenericValueLanguage>>>();
            foreach (var entity in entities)
            {
                var data = await GetData(entity, selecaoId, cancellationToken);

                var memoryTable = new Dictionary<int, IDictionary<string, GenericValueLanguage>>();
                foreach (var item in data)
                {
                    var memoryItem = new Dictionary<string, GenericValueLanguage>();
                    foreach (var property in item.GetType().GetProperties().Where(p => entity.Columns.Any(c => c.Property == p.Name)))
                        memoryItem.Add(property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name, new GenericValueLanguage(property.GetValue(item)));

                    var primaryKey = entity.Columns.FirstOrDefault(c => c.IsPrimaryKey);
                    int identification = (int)item.GetType().GetProperties().FirstOrDefault(p => p.Name == primaryKey.Property)?.GetValue(item);
                    memoryTable.Add(identification, memoryItem);
                }
                tableData.Add(entity.Name, memoryTable);
            }

            var memory = new Dictionary<int, IDictionary<string, GenericValueLanguage>>();
            foreach (var p in tableData[principal])
            {
                var tableMemory = new Dictionary<string, GenericValueLanguage>
                {
                    { $"@{principal}", new GenericValueLanguage(p.Value) }
                };

                if (tables.Any(t => relacionadasPrincipal.Any(rp => rp.Name == t.Name)))
                {
                    foreach (var relacionadaPrincipal in relacionadasPrincipal)
                    {
                        var relacionadaKey = tables.FirstOrDefault(t => t.Name == relacionadaPrincipal.Name).Columns.FirstOrDefault(c => c.IsPrimaryKey);
                        p.Value.TryGetValue(relacionadaKey.Name, out GenericValueLanguage genericValue);
                        tableData[relacionadaPrincipal.Name].TryGetValue((int)genericValue, out IDictionary<string, GenericValueLanguage> val);
                        tableMemory.Add($"@{relacionadaPrincipal.Name}", new GenericValueLanguage(val));
                    }
                }
                memory.Add(p.Key, tableMemory);
            }

            return memory;
        }

        public async Task<IEnumerable<object>> GetData(Table table, int selectionId, CancellationToken cancellationToken = default) => table.Class switch
        {
            nameof(Contribuinte) => await _contribuinteRepository.GetContribuintes(table.Columns.Select(c => c.Property).ToArray(), selectionId, cancellationToken),
            nameof(Fisico) => await _fisicoRepository.GetFisicos(table.Columns.Select(c => c.Property).ToArray(), selectionId, cancellationToken),
            nameof(FaceQuadra) => await _faceQuadraRepository.GetFacesQuadra(table.Columns.Select(c => c.Property).ToArray(), selectionId, cancellationToken),
            _ => throw new BadRequestException("Tabela não configurada para o Cálculo")
        };

        private static void ValidateSearch(Table table, Table entity)
        {
            var notFoundColumns = table.Columns.Except(entity.Columns, new ColumnComparer());
            if (notFoundColumns.Any())
                throw new BadRequestException($"As colunas {string.Join(",", notFoundColumns.Select(c => c.Name))} não foram encontradas para a tabela {table.Name}");
        }
    }
}
