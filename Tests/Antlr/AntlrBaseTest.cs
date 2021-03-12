using Core.Implementation;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests.Antlr
{
    public class AntlrBaseTest
    {
        protected readonly ExecuteLanguage _executeLanguage;

        public IDictionary<string, GenericValueLanguage> Memory { get; private set; }

        public AntlrBaseTest()
        {
            _executeLanguage = new ExecuteLanguage();
        }

        [SetUp]
        public void Setup()
        {
            Memory = new Dictionary<string, GenericValueLanguage>();
        }
    }
}
