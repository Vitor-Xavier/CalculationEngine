using System.Collections;

namespace Tests.Antlr.Functions.Abs
{
    class AbsValidCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] { "var teste = 10; retorno _ABS(teste);", 10 };
            yield return new object[] { "var teste = 10; retorno _ABS(teste - 42);", 32 };
            yield return new object[] { "var teste = -12.6; retorno _ABS(teste);", 12.6m };
            yield return new object[] { "var teste = _ABS(-31); retorno teste;", 31 };
        }
    }
}
