using Ref.Core;
using Ref.Core.VM.IO;
using Ref.Core.VM.IO.Devices;
using System.IO;
using System.Linq;

namespace RefVM_Runner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var filename = args.First();
            var ass = Assembly.Load(File.ReadAllBytes(filename));
            var vm = new VirtualMachine(ass);

            VideoDevice.Enable_ConsoleMode();

            vm.Run();

            VideoDevice.CleanUP();
        }
    }
}