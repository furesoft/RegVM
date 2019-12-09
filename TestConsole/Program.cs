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

            var ass = new CommandWriter {
                { OpCode.LOAD, Registers.A, 0x2A },
                { OpCode.INC, Registers.A },
                { OpCode.LOADRO, 0x0, (int)Registers.D },
                { OpCode.PUSHRO, 0x0},

                { OpCode.OUT, 0xFFAA, (100 << 16) | ((100) & 0xffff) }, // init size of video
                { OpCode.OUT, 0xFFAB, (10 << 16) | ((10) & 0xffff)}, // init position of video
                { OpCode.OUT, 0xFFAF, 1 } // change to videmode
            };

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    ass.Add(OpCode.OUT, 0xFFFF + i + j, 0xFFFFFF);
                }
            }

            ass.Add(OpCode.OUT, 0xFFAF, 2); //Flush Buffer to screen

            var endless = ass.MakeLabel();
            //ass.Add(OpCode.JMP, endless);
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