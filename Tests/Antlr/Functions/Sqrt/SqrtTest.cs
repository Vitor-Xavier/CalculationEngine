using Common.Exceptions;
using NUnit.Framework;

namespace Tests.Antlr.Functions.Sqrt
{
    class SqrtTest : AntlrBaseTest
    {
        [Test]
        [TestCaseSource(typeof(SqrtValidCases))]
        public void ValidSqrt(string text, object expectedResult)
        {
            var parseTree = _executeLanguage.GetParserTree(text);
            var result = _executeLanguage.ExecuteParseTree(parseTree, Memory);

            Assert.AreEqual(expectedResult, result.Value);
        }

        [Test]
        [TestCase("retorno _RAIZ();")]
        [TestCase("retorno _RAIZ(nulo);")]
        public void InvalidSqrt(string text)
        {
            var parseTree = _executeLanguage.GetParserTree(text);
            Assert.Throws<LanguageException>(() => _executeLanguage.ExecuteParseTree(parseTree, Memory));
        }
    }
}
