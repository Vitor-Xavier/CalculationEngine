using System;
using System.Collections.Generic;

namespace Api
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ExecuteLanguage execute = new ExecuteLanguage(@"
                var a = 10 * 18;
                var b = 9.56;
                var c = a + b;
                se (c == @m2) {
                    a = 1;
                    retorno 10;
                    var r = 9.56;
                    }
                var t = 10;
                retorno a;");
            Console.WriteLine("---");
            Console.WriteLine(execute.Execute(new Dictionary<string, GenericValueLanguage> { { "@m2", new GenericValueLanguage(189.56, false) } }));
        }
    }
}
