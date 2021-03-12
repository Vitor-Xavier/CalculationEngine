using NUnit.Framework;
using System;

namespace Tests.Antlr.Functions.IsNull
{
    class IsNullTest : AntlrBaseTest
    {
        [Test]
        [TestCaseSource(typeof(IsNullValidCases))]
        public void ValidIsNull(string text, object expectedResult)
        {
            var parseTree = _executeLanguage.GetParserTree(text);
            var result = _executeLanguage.ExecuteParseTree(parseTree, Memory);

            Assert.AreEqual(expectedResult, result.Value);
        }

        [Test]
        [TestCase("retorno _NULO;")]
        public void InvalidSqrt(string text)
        {
            var parseTree = _executeLanguage.GetParserTree(text);
            Assert.Throws<NullReferenceException>(() => _executeLanguage.ExecuteParseTree(parseTree, Memory));
        }
    }
}
