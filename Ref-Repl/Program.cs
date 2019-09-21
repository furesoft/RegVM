using System;

namespace RefVM
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