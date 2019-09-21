using System;

namespace Ref_Repl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var repl = new Repl();
            repl.Run();

            Console.ReadLine();
        }
    }
}