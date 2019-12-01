using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class EqualInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.EQUAL;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var eq_f = (Registers)(int)cmd[0];
            var eq_s = (Registers)(int)cmd[1];

            vm.Register[Registers.BRR] = eq_f == eq_s ? 1 : 0;
        }
    }
}