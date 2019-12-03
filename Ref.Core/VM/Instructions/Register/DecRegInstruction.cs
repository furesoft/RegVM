using Ref.Core.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ref.Core.VM.Instructions.Register
{
    internal class DecRegisterInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.DEC;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var reg = (Registers)(int)cmd[0];
            vm.Register[reg]--;
        }
    }
}