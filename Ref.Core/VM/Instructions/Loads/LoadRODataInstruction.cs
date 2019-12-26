using System;
using System.IO;
using LibObjectFile.Elf;
using Ref.Core.Parser;
using Ref.Core.VM.IO;

namespace Ref.Core.VM.Instructions
{
    internal class LoadRODataInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.LOADRO;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var startIndex = (int)cmd[0];
            var reg = (Registers)(int)cmd[1];

            var section = vm.Assembly.GetSection<ElfCustomSection>(ElfSectionSpecialType.ReadOnlyData);
            var ms = new MemoryStream();
            section.Stream.CopyTo(ms);

            //ToDo: improve reading ro data
            var value = BitConverter.ToInt32(ms.ToArray(), startIndex);

            vm.Register[reg] = value;
        }
    }
}