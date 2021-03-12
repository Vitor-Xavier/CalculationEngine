using Common.Exceptions;
using NUnit.Framework;

namespace Tests.Antlr.Functions.Abs
{
    class AbsTest : AntlrBaseTest
    {
        [Test]
        [TestCaseSource(typeof(AbsValidCases))]
        public void ValidAbs(string text, object expectedResult)
        {
            var parseTree = _executeLanguage.GetParserTree(text);
            var result = _executeLanguage.ExecuteParseTree(parseTree, Memory);

            Assert.AreEqual(expectedResult, result.Value);
        }

        [Test]
        [TestCase("retorno _ABS();")]
        [TestCase("retorno _ABS(nulo);")]
        public void InvalidSqrt(string text)
        {
            var parseTree = _executeLanguage.GetParserTree(text);
            Assert.Throws<LanguageException>(() => _executeLanguage.ExecuteParseTree(parseTree, Memory));
        }
    }
}
