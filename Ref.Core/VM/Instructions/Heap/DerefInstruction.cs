using Ref.Core.Parser;
using System;

namespace Ref.Core.VM.Instructions
{
    internal class DerefInstruction : Instruction
    {
        public override OpCode OpCode => OpCode.DEREF;

        public override void Invoke(AsmCommand cmd, VirtualMachine vm)
        {
            var addr = vm.Register[Registers.HEAP];
            var value_reg = (Registers)(int)cmd[0];

            unsafe
            {
                vm.Register[value_reg] = *(int*)new IntPtr(addr);
            }
        }
    }
}