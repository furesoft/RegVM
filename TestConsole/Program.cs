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
            ass.Add(OpCode.PRINT);
            ass.Add(OpCode.OUT, 0xABC, 0); //clear console

            ass.Add(OpCode.PUSHL, 9);
            ass.Add(OpCode.OUT, 0xABC, 2); // change foreground

            ass.Add(OpCode.PUSHL, 10);
            ass.Add(OpCode.OUT, 0xABC, 3); // change background

            ass.Add(OpCode.PUSHL, 'e');
            ass.Add(OpCode.OUT, 0xABC, 1); // write e to console

            ass.Add(OpCode.PUSHL, ':');
            ass.Add(OpCode.OUT, 0xABC, 1); // write : to console

            ass.Add(OpCode.IN, 0xABC1, (int)Registers.C); // wait for input char

            // if input is a
            ass.Add(OpCode.LOAD, (int)Registers.B, 65);
            ass.Add(OpCode.EQUAL, (int)Registers.B, (int)Registers.C);
            ass.Add(OpCode.JMPNE, 0x100);
            ass.Add(OpCode.PUSH, (int)Registers.C);
            ass.Add(OpCode.OUT, 0xABC, 1); // write input char to console

            ass.Add(OpCode.PUSHL, '\n'); // write new line to console
            ass.Add(OpCode.OUT, 0xABC, 1);

            ass.Add(OpCode.OUT, 0xABC, 4); // Reset colors

            //Beep
            ass.Add(OpCode.PUSHL, 15000);
            ass.Add(OpCode.PUSHL, 1500);
            ass.Add(OpCode.OUT, 0xABC, 5);
            //.Add(OpCode.CALL, loop);

            var vm = new VirtualMachine();
            vm.Run(ass.Save(), 0);

            Console.WriteLine("Register: " + vm.ViewMemoryOf<Register>().ToHex());
            Console.WriteLine("Stack: " + vm.ViewMemoryOf<Stack>().ToHex());

            Console.ReadLine();
        }
    }
}