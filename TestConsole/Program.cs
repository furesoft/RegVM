using Ref.Core;
using Ref.Core.VM.IO;
using System;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var ass = new CommandWriter();
            ass.Add(OpCode.MOV, (int)Registers.A, 0xC00FFEE);
            ass.Add(OpCode.MOV, (int)Registers.B, 0x2A);

            var vm = new VirtualMachine();
            vm.Run(ass.Save());
        }
    }
}