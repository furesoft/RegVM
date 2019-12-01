using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class PushInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.PUSH;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var from = (int)cmd[0];
            vm.Stack.Push(vm.Register[(Registers)from]);
        }
    }
}