using System;
using System.Collections.Generic;
using System.Text;
using Ref.Core.Parser;

namespace Ref.Core.VM.Instructions
{
    internal class AddInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.ADD;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var add_left = vm.Register[(Registers)(int)cmd[0]];
            var add_right = vm.Register[(Registers)(int)cmd[1]];

            vm.Register[Registers.ACC] = add_left + add_right;
        }
    }
}