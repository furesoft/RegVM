using LibObjectFile.Elf;
using Ref.Core.Parser;
using Ref.Core.VM.IO;
using System;
using System.IO;

namespace Ref.Core.VM.Instructions
{
    internal class PushRODataInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.PUSHRO;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var startIndex = (int)cmd[0];

            var section = vm.Assembly.GetSection<ElfCustomSection>(ElfSectionSpecialType.ReadOnlyData);
            var ms = new MemoryStream();
            section.Stream.CopyTo(ms);

            //ToDo: improve reading ro data
            var value = BitConverter.ToInt32(ms.ToArray(), startIndex);

            vm.Stack.Push(value);
        }
    }
}