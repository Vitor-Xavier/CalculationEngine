using System;

namespace Api
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ExecuteLanguage execute = new ExecuteLanguage("var a = 10 + 12;var b = 1 + 12;retorno teste;");

            Console.WriteLine(execute.Execute());


        }
    }
}
