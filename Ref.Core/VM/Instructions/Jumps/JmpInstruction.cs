using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class JmpInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.JMP;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var addr = (int)cmd[0];
            vm.Register[Registers.IPR] = addr;
        }
    }
}