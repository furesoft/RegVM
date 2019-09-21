using RefVM;
using System.IO;
using System.Linq;

namespace RefVM_Runner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var filename = args.First();
            var vm = new VM();

            vm.Run(File.ReadAllBytes(filename));
        }
    }
}