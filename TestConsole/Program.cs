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

            //inc-method at 0
            ass.Add(OpCode.LOAD, (int)Registers.A, 0x2A);
            ass.Add(OpCode.LOAD, (int)Registers.B, 1);

            var loop = ass.MakeLabel();
            ass.Add(OpCode.ADD, (int)Registers.A, (int)Registers.B);
            ass.Add(OpCode.MOV, (int)Registers.ACC, (int)Registers.A);
            ass.Add(OpCode.LOAD, (int)Registers.D, 65);
            ass.Add(OpCode.PRINT);
            ass.Add(OpCode.PUSH, (int)Registers.D);
            ass.Add(OpCode.OUT, 0xABC, 0);
            ass.Add(OpCode.OUT, 0xABC, 1);
            ass.Add(OpCode.LOAD, (int)Registers.D, (int)'\n');
            ass.Add(OpCode.PUSH, (int)Registers.D);
            ass.Add(OpCode.OUT, 0xABC, 1);
            //.Add(OpCode.CALL, loop);

            var vm = new VirtualMachine();
            vm.Run(ass.Save(), 0);

            vm.Stack.PushRegisters(vm.Register);
            vm.Register.ClearRegister(Registers.A);
            vm.Stack.PopRegisters(vm.Register);

            Console.WriteLine("Register: " + vm.ViewMemoryOf<Register>().ToHex());
            Console.WriteLine("Stack: " + vm.ViewMemoryOf<Stack>().ToHex());

            Console.ReadLine();
        }
    }
}