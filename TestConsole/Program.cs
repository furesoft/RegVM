using Ref.Core;
using Ref.Core.VM.IO;
using System;
using Ref.Core.VM.IO.Devices;
using Ref.Core.VM.Core;
using LibObjectFile.Elf;
using System.IO;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            VideoDevice.Enable_ConsoleMode();

            var file = new AssemblyWriter();
            var meta = new AssemblyInfo { Version = "1.0.0.0", ID = Guid.NewGuid() };

            file.CreateMetaSection(meta);

            var ro = file.Elf;
            ro.AddSection(new ElfCustomSection(new MemoryStream(BitConverter.GetBytes(0x2A))).ConfigureAs(ElfSectionSpecialType.ReadOnlyData));

            var ass = new CommandWriter();

            ass.Add(OpCode.OUT, 0xFFAA, (100 << 16) | ((100) & 0xffff)); // init size of video
            ass.Add(OpCode.OUT, 0xFFAB, (10 << 16) | ((10) & 0xffff)); // init position of video
            ass.Add(OpCode.OUT, 0xFFAF, 1); // change to videmode
            ass.Add(OpCode.OUT, 0xBCD, 1); // initialize keyboard device

            var check = ass.MakeLabel();
            ass.Add(OpCode.IN, 0xBCD1, 1);
            ass.Add(OpCode.EQUAL, Registers.KDS, 0x01);
            ass.Add(OpCode.JMPNE, check);
            ass.Add(OpCode.PUSH, Registers.KDS);
            ass.Add(OpCode.INT, 0x1);
            ass.Add(OpCode.JMP, check);

            /*
            for (short i = 0; i < 100; i++)
            {
                for (short j = 0; j < 100; j++)
                {
                    var address = i << 16 | j;
                    Pixel color;
                    if (i >= 50 || j >= 50)
                    {
                        color = Pixels.Blue;
                    }
                    else
                    {
                        color = Pixels.Red;
                    }
                    ass.Add(OpCode.OUT, 0xFFFF + address, color.ToHex());
                }
            }*/

            var endless = ass.MakeLabel();
            ass.Add(OpCode.OUT, 0xFFAF, 2); //Flush Buffer to screen

            ass.Add(OpCode.JMP, endless);
            //.Add(OpCode.CALL, loop);

            file.CreateCodeSection(ass);

            var vm = new VirtualMachine(ElfObjectFile.Read(new MemoryStream(file.Save())));
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