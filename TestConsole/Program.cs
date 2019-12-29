﻿using Ref.Core;
using Ref.Core.VM.IO;
using System;
using Ref.Core.VM.IO.Devices;
using Ref.Core.VM.Core;
using LibObjectFile.Elf;
using System.IO;
using Ref.Core.Parser;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            VideoDevice.Enable_ConsoleMode();

            var src = ".db 42\nLOAD8 0x2A, $A";
            var pr = AsmParser.Parse(src);

            //VideoDevice.DefaultContext = new MonoDrawingContext();

            var file = new AssemblyWriter();
            var meta = new AssemblyInfo { Version = "1.0.0.0", ID = Guid.NewGuid() };

            file.CreateMetaSection(meta);

            var ro = file.Elf;
            ro.AddSection(new ElfCustomSection(new MemoryStream(BitConverter.GetBytes(0x2A))).ConfigureAs(ElfSectionSpecialType.ReadOnlyData));

            var ass = new CommandWriter();

            ass.Add(OpCode.NEW, 1024);
            ass.Add(OpCode.FREE);

            //ass.Add(OpCode.OUT, 0xFFAA, (500 << 16) | ((500) & 0xffff)); // init size of video
            //ass.Add(OpCode.OUT, 0xFFAB, (100 << 16) | ((100) & 0xffff)); // init position of video
            /*ass.Add(OpCode.OUT, 0xFFAF, 1); // change to videmode

            for (short i = 0; i < 100; i++)
            {
                for (short j = 0; j < 100; j++)
                {
                    var address = i << 16 | j;
                    Pixel color = Pixels.Red;

                    ass.Add(OpCode.OUT, 0xFFFF + address, color.ToHex());
                }
            }

            ass.Add(OpCode.OUT, 0xFFAF, 2); //Flush Buffer to screen

            //.Add(OpCode.CALL, loop);

            */

            file.CreateCodeSection(ass);

            var vm = new VirtualMachine(ElfObjectFile.Read(new MemoryStream(file.Save())));
            vm.Run();
            vm.SetMemoryOf<Register>(0, 0x2a);

            Console.ReadLine();

            /*Utils.PrintRegisters(vm.Register);

            Console.WriteLine("Register: " + vm.ViewMemoryOf<Register>(50).ToHex());
            Console.WriteLine("Stack: " + vm.ViewMemoryOf<Stack>(50).ToHex());

            Console.ReadLine();
            */
            VideoDevice.CleanUP();
        }
    }
}