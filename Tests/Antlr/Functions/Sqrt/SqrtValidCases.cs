using System.Collections;

namespace Tests.Antlr.Functions.Sqrt
{
    class SqrtValidCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] { "var teste = 9; retorno _RAIZ(teste);", 3 };
            yield return new object[] { "var teste = 25; retorno _RAIZ(teste);", 5 };
            yield return new object[] { "var teste = 100; retorno _RAIZ(teste);", 10 };
            yield return new object[] { "var teste = 12; retorno _RAIZ(teste);", 3.4641016151377544 };
            yield return new object[] { "retorno _RAIZ(4);", 2 };
        }
    }
}