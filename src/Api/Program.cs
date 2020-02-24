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
                    retorno b;
                }
                retorno a;");

            Console.WriteLine(execute.Execute(new Dictionary<string, GenericValueLanguage> { { "@m2", new GenericValueLanguage(189.56, false) } }));
        }
    }
}
