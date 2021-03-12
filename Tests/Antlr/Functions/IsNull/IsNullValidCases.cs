using System.Collections;

namespace Tests.Antlr.Functions.IsNull
{
    class IsNullValidCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] { "var teste = nulo; retorno _NULO(teste);", true };
            yield return new object[] { "var teste = \"teste\"; retorno _NULO(teste);", false };
            yield return new object[] { "var teste = 12.6; retorno _NULO(teste);", false };
            yield return new object[] { "var teste = 01/01/2021; retorno _NULO(teste);", false };
            yield return new object[] { "var teste = 10; retorno _NULO(teste);", false };
            yield return new object[] { "var teste = true; retorno _NULO(teste);", false };
            yield return new object[] { "lista teste = []; retorno _NULO(teste);", false };
        }
    }
}
