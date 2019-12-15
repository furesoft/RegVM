using System;
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

            /* var section = vm.Assembly[AssemblySections.ReadOnly];
             var value = BitConverter.ToInt32(section.Raw, startIndex);

             vm.Register[reg] = value;*/
        }
    }
}