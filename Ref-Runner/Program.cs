using LibObjectFile.Elf;
using Ref.Core;
using Ref.Core.VM.IO;
using Ref.Core.VM.IO.Devices;
using System.IO;
using System.Linq;
using TestConsole;

namespace RefVM_Runner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var filename = args.First();
            var ass = ElfObjectFile.Read(File.Open(filename, FileMode.OpenOrCreate));
            var vm = new VirtualMachine(ass);

            VideoDevice.Enable_ConsoleMode();
            VideoDevice.DefaultContext = new WinformsDrawingContext();

            vm.Run();

            VideoDevice.CleanUP();
        }
    }
}