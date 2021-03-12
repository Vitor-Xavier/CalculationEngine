using Common.Exceptions;
using NUnit.Framework;

namespace Tests.Antlr.Functions.Round
{
    class RoundTest : AntlrBaseTest
    {
        [Test]
        [TestCaseSource(typeof(RoundValidCases))]
        public void ValidRound(string text, object expectedResult)
        {
            var parseTree = _executeLanguage.GetParserTree(text);
            var result = _executeLanguage.ExecuteParseTree(parseTree, Memory);

            Assert.AreEqual(expectedResult, result.Value);
        }

        [Test]
        [TestCase("retorno _ARREDONDAR();")]
        [TestCase("retorno _ARREDONDAR(nulo);")]
        public void InvalidSqrt(string text)
        {
            var parseTree = _executeLanguage.GetParserTree(text);
            Assert.Throws<LanguageException>(() => _executeLanguage.ExecuteParseTree(parseTree, Memory));
        }
    }
}
