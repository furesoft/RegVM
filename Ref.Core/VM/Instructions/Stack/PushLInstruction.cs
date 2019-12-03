using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class PushLInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.PUSHL;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var push_lit = (int)cmd[0];
            vm.Stack.Push(push_lit);
        }
    }
}