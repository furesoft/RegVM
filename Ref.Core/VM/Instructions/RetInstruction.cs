using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;
using Ref.Core.VM.Core;

namespace Ref.Core.VM.Instructions
{
    internal class RetInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.RET;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            vm.Stack.PopRegisters(vm.Register);
        }
    }
}