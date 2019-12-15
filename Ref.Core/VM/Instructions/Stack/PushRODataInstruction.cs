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

            //ToDo: improve reading ro data
            var value = BitConverter.ToInt32(((MemoryStream)section.Stream).ToArray(), startIndex);

            vm.Stack.Push(value);
        }
    }
}