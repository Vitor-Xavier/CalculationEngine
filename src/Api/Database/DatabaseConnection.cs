using Api.Dto;
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
                    var tableResults = new List<object>();
                    var table = queries.ElementAt(j).Tabela;
                    while (await reader.ReadAsync())
                    {
                        var obj = new ExpandoObject() as IDictionary<string, object>;
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string aliasName = reader.GetName(i);
                            string columnName = aliasName.Substring(aliasName.LastIndexOf('.') + 1);
                            string tableName = aliasName.Contains('.') ? aliasName.Substring(0, aliasName.LastIndexOf('.')) : table;
                            object columnValue = await reader.IsDBNullAsync(i) ? null : reader[i];
                            if (tableName != table)
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

        public async Task<IEnumerable<object>> GetData(string sql)
        {
            var results = new List<object>();
            SqlDataReader reader = null;
            try
            {
                await _connection.OpenAsync();

                SqlCommand command = new SqlCommand(sql, _connection);
                reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var obj = new ExpandoObject() as IDictionary<string, object>;
                    for (int i = 0; i < reader.FieldCount; i++)
                        obj.Add(reader.GetName(i), reader[reader.GetName(i)]);
                    results.Add(obj);
                }
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
