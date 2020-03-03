using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Threading.Tasks;
using Api.Dto;

namespace Api.Database
{
  public class DatabaseConnection
  {
    private readonly SqlConnection _connection;

    public DatabaseConnection()
    {
      _connection = new SqlConnection("Server=192.168.0.132,1433;Database=SMARTB_PMBotucatu;User Id=smar;Password=smarapd");
    }

    public async Task<IDictionary<string, IEnumerable<object>>> GetAllData(string[] tabelas, string sql)
    {
      var results = new Dictionary<string, IEnumerable<object>>();
      SqlDataReader reader = null;
      try
      {
        await _connection.OpenAsync();

        SqlCommand command = new SqlCommand(sql, _connection);
        reader = await command.ExecuteReaderAsync();
        int j = 0;
        do
        {
          var tableResults = new List<object>();

          while (await reader.ReadAsync())
          {
            var obj = new ExpandoObject() as IDictionary<string, object>;
            for (int i = 0; i < reader.FieldCount; i++)
              obj.Add(reader.GetName(i), await reader.IsDBNullAsync(i) ? null : reader[i]);
            tableResults.Add(obj);
          }
          results.Add(tabelas[j++], tableResults);
        } while (await reader.NextResultAsync());
      }
      finally
      {
        reader?.Close();
        _connection.Close();
      }
      return results;
    }

    public async Task<IDictionary<string, IEnumerable<object>>> GetAllDataCaracteristica(CaracteristicaParametros[] tabelas, string sql)
    {
      var results = new Dictionary<string, IEnumerable<object>>();
      SqlDataReader reader = null;
      try
      {
        await _connection.OpenAsync();

        SqlCommand command = new SqlCommand(sql, _connection);
        reader = await command.ExecuteReaderAsync();
        int w = 0;
        do
        {
          var tableResults = new List<object>();

          while (await reader.ReadAsync())
          {
            var obj = new ExpandoObject() as IDictionary<string, object>;
            for (int i = 0; i < reader.FieldCount; i++)
              obj.Add(reader.GetName(i), await reader.IsDBNullAsync(i) ? null : reader[i]);
            tableResults.Add(obj);
          }

          results.Add((tabelas[w].TabelaCaracteristica + "." + tabelas[w].DescricaoCaracteristica).ToString(), tableResults);
          w++;
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
