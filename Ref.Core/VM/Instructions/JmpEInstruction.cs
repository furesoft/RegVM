using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class JmpEInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.JMPE;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var e_addr = (int)cmd[0];
            if (vm.Register[Registers.BRR] != 0)
            {
                vm.Register[Registers.IPR] = e_addr;
            }
        }
    }
}