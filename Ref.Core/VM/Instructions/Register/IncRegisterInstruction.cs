using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions.Register
{
    internal class IncRegisterInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.INC;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var reg = (Registers)(int)cmd[0];
            vm.Register[reg]++;
        }
    }
}