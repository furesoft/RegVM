using Ref.Core;
using Ref.Core.VM.IO;
using Ref.Core.VM.Core;
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
            ass.Add(OpCode.PUSH, (int)Registers.B);
            ass.Add(OpCode.PRINT);

            var vm = new VirtualMachine();
            vm.Run(ass.Save());

            Console.WriteLine("Register: " + vm.ViewMemoryOf<Register>().ToHex());
            Console.WriteLine("Stack: " + vm.ViewMemoryOf<Stack>().ToHex());

            Console.ReadLine();
        }
    }
}