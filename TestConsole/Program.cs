using Ref.Core;
using Ref.Core.VM.IO;
using Ref.Core.VM.Core;
using System;
using Ref.Core.VM.IO.Devices;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            VideoDevice.Enable_ConsoleMode();

            var file = new AssemblyWriter();
            var meta = new AssemblyInfo { Version = "1.0.0.0", ID = Guid.NewGuid() };

            file.AddMeta(meta);

            var typeinfo = file.CreateSection(AssemblySections.TypeInfo);
            var debuginfo = file.CreateSection(AssemblySections.DebugInfo);
            var ro = file.CreateSection(AssemblySections.ReadOnly);
            var data = file.CreateSection(AssemblySections.Code);

            ro.Raw = BitConverter.GetBytes(0x2A);

            var ass = new CommandWriter();

            //inc-method at 0
            ass.Add(OpCode.LOAD, (int)Registers.A, 0x2A);
            ass.Add(OpCode.INC, (int)Registers.A);
            ass.Add(OpCode.LOADRO, 0x0, (int)Registers.D);
            ass.Add(OpCode.PUSHRO, 0x0);

            ass.Add(OpCode.OUT, 0xABCD1, 'h');
            ass.Add(OpCode.OUT, 0xABCD2, 'e');
            ass.Add(OpCode.OUT, 0xABCD3, 'l');
            ass.Add(OpCode.OUT, 0xABCD4, 'l');
            ass.Add(OpCode.OUT, 0xABCD5, 'o');

            ass.Add(OpCode.PUSHL, 9);
            ass.Add(OpCode.OUT, 0xABC, 2); // change foreground

            ass.Add(OpCode.PUSHL, 10);
            ass.Add(OpCode.OUT, 0xABC, 3); // change background

            ass.Add(OpCode.PUSHL, 'e');
            ass.Add(OpCode.OUT, 0xABC, 1); // write e to console

            ass.Add(OpCode.PUSHL, ':');
            ass.Add(OpCode.OUT, 0xABC, 1); // write : to console

            var inputloop = ass.MakeLabel();
            ass.Add(OpCode.IN, 0xABC1, (int)Registers.C); // wait for input char
            // ass.Add(OpCode.JMP, inputloop);
            ass.Add(OpCode.OUT, 0xFFAA, (500 << 16) | ((1500) & 0xffff)); // init size of video
            ass.Add(OpCode.OUT, 0xFFAB, (100 << 16) | ((100) & 0xffff)); // init position of video
            ass.Add(OpCode.OUT, 0xFFAF, 1); // change to videmode
            ass.Add(OpCode.OUT, 0xFFFF + 10, 0xFFFFFF);
            //Beep
            ass.Add(OpCode.PUSHL, 1500);
            ass.Add(OpCode.PUSHL, 1500);
            ass.Add(OpCode.OUT, 0xABC, 5);

            var endless = ass.MakeLabel();
            ass.Add(OpCode.JMP, endless);
            //.Add(OpCode.CALL, loop);

            data.Raw = ass.Save();
            var vm = new VirtualMachine(Assembly.Load(file.Save()));
            vm.Run();

            /*Utils.PrintRegisters(vm.Register);

            Console.WriteLine("Register: " + vm.ViewMemoryOf<Register>(50).ToHex());
            Console.WriteLine("Stack: " + vm.ViewMemoryOf<Stack>(50).ToHex());

            Console.ReadLine();
            */
            VideoDevice.CleanUP();
        }
    }
}