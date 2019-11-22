using Ref.Core;
using System.IO;
using System.Linq;

namespace RefVM_Runner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var filename = args.First();
            var vm = new VirtualMachine();

            vm.Run(File.ReadAllBytes(filename));
        }
    }
}