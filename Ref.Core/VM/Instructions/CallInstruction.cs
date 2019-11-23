using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;
using Ref.Core.VM.Core;

namespace Ref.Core.VM.Instructions
{
    internal class CallInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.CALL;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var cll = (int)cmd[0];
            vm.Stack.PushRegisters(vm.Register);
            vm.Register[Registers.IPR] = cll;
        }
    }
}