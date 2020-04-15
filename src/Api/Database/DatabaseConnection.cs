using Api.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Database
{
    public class DatabaseConnection : IAsyncDisposable
    {
        private readonly SqlConnection _connection;

        public DatabaseConnection() =>
            _connection = new SqlConnection("");

        private async Task Open()
        {
            if (_connection.State == ConnectionState.Closed)
                await _connection.OpenAsync();
        }

        private async Task Close()
        {
            if (_connection.State == ConnectionState.Open)
                await _connection.CloseAsync();
        }

        public async Task<IDictionary<string, IEnumerable<IDictionary<string, GenericValueLanguage>>>> GetAllData(IEnumerable<TabelaQuery> queries)
        {
            var results = new Dictionary<string, IEnumerable<IDictionary<string, GenericValueLanguage>>>();
            SqlDataReader reader = null;
            try
            {
                await Open();
                SqlCommand command = new SqlCommand(string.Join(";", queries.Select(x => x.Consulta)), _connection);
                reader = await command.ExecuteReaderAsync();
                int j = 0;
                do
                {
                    var tableResults = new List<IDictionary<string, GenericValueLanguage>>();
                    var table = queries.ElementAt(j).Tabela;
                    while (await reader.ReadAsync())
                    {
                        var obj = new Dictionary<string, GenericValueLanguage>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string aliasName = reader.GetName(i);
                            string columnName = aliasName.Substring(aliasName.LastIndexOf(".", StringComparison.Ordinal) + 1);
                            string tableName = aliasName.Contains('.', StringComparison.Ordinal) ? aliasName.Substring(0, aliasName.LastIndexOf(".", StringComparison.Ordinal)) : table;
                            GenericValueLanguage columnValue = new GenericValueLanguage(await reader.IsDBNullAsync(i) ? null : reader[i]);
                            if (!tableName.Equals(table, StringComparison.Ordinal))
                            {
                                if (!obj.ContainsKey(tableName))
                                {
                                    (obj as Dictionary<string, GenericValueLanguage>).Add(tableName + "." + columnName, columnValue);
                                }

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
                await Close();
            }
            return results;
        }

        public async Task<IDictionary<string, IEnumerable<object>>> GetAllDataCaracteristica(IEnumerable<TabelaQuery> queries)
        {
            var results = new Dictionary<string, IEnumerable<object>>();
            var tableResults = new List<object>();
            SqlDataReader reader = null;
            try
            {
                await Open();

                SqlCommand command = new SqlCommand(string.Join(";", queries.Select(x => x.Consulta)), _connection);
                reader = await command.ExecuteReaderAsync();
                int j = 0;
                do
                {

                    var table = queries.ElementAt(j).Tabela;
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
                    results.Add(table + "[" + j + "]", tableResults);
                } while (await reader.NextResultAsync());
            }
            finally
            {
                reader?.Close();
                await Close();
            }

            return results;
        }

        public async Task<List<IDictionary<string, object>>> Sql(string query)
        {
            var results = new Dictionary<string, IEnumerable<object>>();
            var tableResults = new List<IDictionary<string, object>>();

            SqlDataReader reader = null;
            try
            {
                await Open();

                SqlCommand command = new SqlCommand(query, _connection);
                reader = await command.ExecuteReaderAsync();
                int j = 0;


                while (await reader.ReadAsync())
                {
                    var obj = new ExpandoObject() as IDictionary<string, object>;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string aliasName = reader.GetName(i);
                        string columnName = aliasName.Substring(aliasName.LastIndexOf(".", StringComparison.Ordinal) + 1);
                        object columnValue = await reader.IsDBNullAsync(i) ? null : reader[i];

                        obj.Add(columnName, columnValue);
                    }
                    tableResults.Add(obj);

                }

            }
            finally
            {
                reader?.Close();
                await Close();
            }

            return tableResults;
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}
