using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Common.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Transforma um dicionário de dados de, nome da propriedade e seu valor, em um objeto.
        /// </summary>
        /// <param name="dictionary">Dicionário de dados</param>
        /// <returns>Objeto Expansível</returns>
        public static object ToObject(this IDictionary<string, object> dictionary) =>
            dictionary.Aggregate(new ExpandoObject() as IDictionary<string, object>, (d, k) => { d.Add(k.Key, k.Value); return d; });
    }
}
