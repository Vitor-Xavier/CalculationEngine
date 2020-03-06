using Api.Dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Database
{
    public class DatabaseConnection
  {
    private readonly SqlConnection _connection;

    public DatabaseConnection()
    {
      _connection = new SqlConnection("");
    }

    public async Task<IDictionary<string, IEnumerable<object>>> GetAllData(IEnumerable<TabelaQuery> queries)
    {
      var results = new Dictionary<string, IEnumerable<object>>();
      SqlDataReader reader = null;
      try
      {
        await _connection.OpenAsync();
        SqlCommand command = new SqlCommand(string.Join(";", queries.Select(x => x.Consulta)), _connection);
        reader = await command.ExecuteReaderAsync();
        int j = 0;
        do
        {
          var table = queries.ElementAt(j).Tabela;
          var tableResults = new HashSet<object>();
          while (await reader.ReadAsync())
          {
            var obj = new ExpandoObject() as IDictionary<string, object>;
            for (int i = 0; i < reader.FieldCount; i++)
            {
              string aliasName = reader.GetName(i);
              string columnName = aliasName.Substring(aliasName.LastIndexOf(".", StringComparison.Ordinal) + 1);
              string tableName = aliasName.Contains('.', StringComparison.Ordinal) ? aliasName.Substring(0, aliasName.LastIndexOf(".", StringComparison.Ordinal)) : table;
              object columnValue = await reader.IsDBNullAsync(i) ? null : reader[i];
              if (!tableName.Equals(table, StringComparison.Ordinal))
              {
                if (!obj.ContainsKey(tableName))
                  obj.Add(tableName, new ExpandoObject() as IDictionary<string, object>);
                (obj[tableName] as IDictionary<string, object>).Add(columnName, columnValue);
              }
              else
                obj.Add(columnName, columnValue);
            }
            tableResults.Add(obj);
          }
          j++;
          results.Add(table, tableResults);
        } while (await reader.NextResultAsync());
      }
      finally
      {
        reader?.Close();
        _connection.Close();
      }
      return results;
    }

    public async Task<IDictionary<string, IEnumerable<object>>> GetAllDataCaracteristica(IEnumerable<TabelaQuery> queries)
    {
      var results = new Dictionary<string, IEnumerable<object>>();
      SqlDataReader reader = null;
      try
      {
        await _connection.OpenAsync();

        SqlCommand command = new SqlCommand(string.Join(";", queries.Select(x => x.Consulta)), _connection);
        reader = await command.ExecuteReaderAsync();
        int j = 0;
        do
        {
          var tableResults = new List<object>();
          var table = queries.ElementAt(j).Tabela;
          
          while (await reader.ReadAsync())
          {
            var obj = new ExpandoObject() as IDictionary<string, object>;
            for (int i = 0; i < reader.FieldCount; i++)
              obj.Add(reader.GetName(i), await reader.IsDBNullAsync(i) ? null : reader[i]);
            tableResults.Add(obj);
          }

          j++;
          results.Add(table, tableResults);
          
        } while (await reader.NextResultAsync());
      }
      finally
      {
        reader?.Close();
        _connection.Close();
      }
      return results;
    }
  }
}
