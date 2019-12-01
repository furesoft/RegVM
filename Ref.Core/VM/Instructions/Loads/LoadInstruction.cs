using Ref.Core.Parser;
using System;

namespace Ref.Core.VM.Instructions
{
    internal class LoadInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.LOAD;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var reg = (int)cmd[0];
            var val = cmd[1];

            vm.Register[(Registers)reg] = (int)val;
        }
    }
}