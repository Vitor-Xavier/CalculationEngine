using System.Collections;

namespace Tests.Antlr.Functions.Round
{
    class RoundValidCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] { "var teste = 12.4; retorno _ARREDONDAR(teste);", 12 };
            yield return new object[] { "var teste = 12.6; retorno _ARREDONDAR(teste);", 13 };
            yield return new object[] { "var teste = 12.6; retorno _ARREDONDAR(teste, 1);", 12.6m };
            yield return new object[] { "var teste = 12.6; retorno _ARREDONDAR(teste, 2);", 12.6m };
            yield return new object[] { "var teste = 12.6; retorno _ARREDONDAR(teste, 3);", 12.6m };
            yield return new object[] { "var teste = 12.6295; retorno _ARREDONDAR(teste, 3);", 12.63m };
            yield return new object[] { "var teste = 12.6294; retorno _ARREDONDAR(teste, 3);", 12.629m };
            yield return new object[] { "var teste = 12.69784; retorno _ARREDONDAR(teste, 3);", 12.698m };
            yield return new object[] { "var teste = 144000.129; retorno _ARREDONDAR(teste, 2);", 144000.13m };
        }
    }
}
