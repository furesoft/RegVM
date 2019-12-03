using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class JmpRInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.JMPR;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var jmp_index = (int)cmd[0];

            if (jmp_index < 0)
            {
                vm.Register[Registers.IPR] -= jmp_index;
            }
            else
            {
                vm.Register[Registers.IPR] += jmp_index;
            }
        }
    }
}