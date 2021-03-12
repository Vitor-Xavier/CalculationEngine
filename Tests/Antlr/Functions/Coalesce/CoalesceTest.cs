using NUnit.Framework;
using System;

namespace Tests.Antlr.Functions.Coalesce
{
    [TestFixture]
    public class CoalesceTest : AntlrBaseTest
    {
        [Test]
        [TestCaseSource(typeof(CoalesceValidCases))]
        public void ValidCoalesce(string text, object expectedResult)
        {
            var parseTree = _executeLanguage.GetParserTree(text);
            var result = _executeLanguage.ExecuteParseTree(parseTree, Memory);

            Assert.AreEqual(expectedResult, result.Value);
        }

        [Test]
        [TestCase("retorno _COALESCE;")]
        public void InvalidCoalesce(string text)
        {
            var parseTree = _executeLanguage.GetParserTree(text);
            Assert.Throws<NullReferenceException>(() => _executeLanguage.ExecuteParseTree(parseTree, Memory));
        }
    }
}
