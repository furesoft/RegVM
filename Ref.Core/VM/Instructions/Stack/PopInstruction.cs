using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class PopInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.POP;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var to = cmd[0];
            var rval = vm.Stack.Pop();

            vm.Register[(Registers)to] = rval;
        }
    }
}