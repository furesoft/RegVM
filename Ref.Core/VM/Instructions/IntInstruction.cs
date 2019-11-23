using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;
using Ref.Core.VM.Core.Interrupts;

namespace Ref.Core.VM.Instructions
{
    internal class IntInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.INT;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var interrupt = (int)cmd[0];
            InterruptTable.Interrupt(interrupt, this);
        }
    }
}