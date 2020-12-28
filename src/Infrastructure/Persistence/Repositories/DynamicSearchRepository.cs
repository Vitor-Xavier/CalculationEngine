using Crosscutting.DTO.DynamicSearch;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class DynamicSearchRepository
    {
        private readonly CalculationContext _context;

        public DynamicSearchRepository(CalculationContext context) => _context = context;

        public Table GetTable<TEntity>() => GetTable(typeof(TEntity));

        public Table GetTable(Type type)
        {
            var entityType = _context.Model.FindEntityType(type);

            return new Table
            {
                Name = entityType.GetTableName(),
                Class = type.Name,
                Columns = entityType.GetProperties().Select(p => new Column
                {
                    Name = p.GetColumnName(),
                    IsPrimaryKey = p.IsPrimaryKey(),
                    Property = p.Name
                }).ToList()
            };
        }

        public Table GetTable(string name) => GetTable(GetTypeByTable(name));

        public Type GetTypeByTable(string table) => typeof(CalculationContext).GetProperties()
            .Where(x => x.PropertyType.IsGenericType && (typeof(DbSet<>).IsAssignableFrom(x.PropertyType.GetGenericTypeDefinition())))
            .Select(x => x.PropertyType.GetGenericArguments()[0]).FirstOrDefault(x => _context.Model.FindEntityType(x).GetTableName() == table);
    }
}
