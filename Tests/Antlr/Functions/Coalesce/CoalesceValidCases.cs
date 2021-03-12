using System.Collections;

namespace Tests.Antlr.Functions.Coalesce
{
    class CoalesceValidCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] { "var teste = nulo; retorno _COALESCE(teste, 10);", 10 };
            yield return new object[] { "var teste = 12.6; retorno _COALESCE(teste, 10);", 12.6m };
            yield return new object[] { "var teste = nulo; retorno _COALESCE(teste, nulo);", null };
        }
    }
}
